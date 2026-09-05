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
    public class AuthService: IAuthService
    {
        IConfiguration _configuracion;
        IUsuarioRepository _usuarios;

        public AuthService(IConfiguration configuracion, IUsuarioRepository usuarios)
        {
            this._configuracion = configuracion;
            this._usuarios = usuarios;
        }

        public async Task<(string,DateTime)?> Login(string mail, string pass)
        {
            var user = await _usuarios.GetByEmailAsync(mail);

            //Incorporar Hash en la pass al momento de registrar el usuario
            //despues acordarnos de incorporar la comparacion con Bcrypt.net
            if (user is null||user.Password!=pass) return null;


            var result = GenerarToken(user);
            return result;
        }

        private (string,DateTime) GenerarToken(Usuario usuario)
        {
            var payloadInfo = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,usuario.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,usuario.Email),
                new Claim(ClaimTypes.Role,usuario.Rol),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(payloadInfo),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuracion["Jwt:Issuer"],
                Audience = _configuracion["Jwt:Audience"],
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token), DateTime.Now.AddHours(2));
        }
    }
}
