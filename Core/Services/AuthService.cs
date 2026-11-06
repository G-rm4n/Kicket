using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Data.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Services
{
    // Servicio de autenticación: verifica credenciales y genera el token JWT.
    public class AuthService : IAuthService
    {
        // Configuración de la app (de acá se leen Jwt:Key, Jwt:Issuer y Jwt:Audience).
        IConfiguration _configuracion;
        // Repositorio para buscar y guardar usuarios.
        IUsuarioRepository _usuarios;

        // Recibe la configuración y el repositorio de usuarios por inyección de dependencias.
        public AuthService(IConfiguration configuracion, IUsuarioRepository usuarios)
        {
            this._configuracion = configuracion;
            this._usuarios = usuarios;
        }

        // Login: busca al usuario por email y verifica la contraseña contra el hash BCrypt.
        // Si falla devuelve null; si sale bien genera el token y lo devuelve junto con el usuario.
        public async Task<(string, DateTime, Usuario)?> Login(string mail, string pass)
        {
            var user = await _usuarios.GetByEmailAsync(mail);

            if (user is null || !Contrasenas.Verificar(pass, user.Password)) return null;


            // Genera el JWT para el usuario y lo devuelve con su fecha de vencimiento.
            var (token, expiraEn) = GenerarToken(user);
            return (token, expiraEn, user);
        }

        // Arma el token JWT firmado con HMAC-SHA256 y vencimiento de 2 horas.
        private (string, DateTime) GenerarToken(Usuario usuario)
        {
            // Claims del token: id del usuario (sub), email, rol y un id único del token (jti).
            var payloadInfo = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Clave secreta (Jwt:Key) con la que se firma el token.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Datos del token: identidad, vencimiento, emisor, audiencia y credenciales de firma.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(payloadInfo),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuracion["Jwt:Issuer"],
                Audience = _configuracion["Jwt:Audience"],
                SigningCredentials = creds
            };

            // Crea el token y lo convierte a texto (el string que viaja en el header Authorization).
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token), DateTime.UtcNow.AddHours(2));
        }


        // Registro: rechaza emails repetidos y contraseñas vacías, hashea la contraseña,
        // asigna el rol "Cliente" y la fecha de alta, y guarda el usuario.
        public async Task RegistrarUsuario(Usuario usuario)
        {
            var EmailDupp = await this._usuarios.ExistsEmail(usuario.Email);

            if (EmailDupp) throw new ArgumentException("El correo provisto Ya se encuentra en uso");
            if (string.IsNullOrWhiteSpace(usuario.Password)) throw new ArgumentException("La contraseña es obligatoria.");
            if (string.IsNullOrWhiteSpace(usuario.Nombre) || string.IsNullOrWhiteSpace(usuario.Apellido))
                throw new ArgumentException("Nombre y apellido son obligatorios.");

            // DNI obligatorio, valido y sin repetir.
            Validaciones.ValidarDatosPersonales(usuario, dniObligatorio: true);
            if (await this._usuarios.ExistsDni(usuario.Dni!))
                throw new ArgumentException("El DNI provisto ya se encuentra registrado.");

            usuario.Password = Contrasenas.Hashear(usuario.Password);
            usuario.Rol = "Cliente";
            usuario.FechaRegistro=DateTime.UtcNow;

            await this._usuarios.AddAsync(usuario);
        }
    }
}