using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kiketpropio.Dominio.DTOs;
using Kiketpropio.Dominio.Entidades;
using Kiketpropio.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Kiketpropio.Services.Implementaciones
{
    public class AuthService:IAuthService
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
        }

        public LoginResponse? Login(LoginRequest loginInput)
        {
            var userFounded = _usuarioService.GetByEmail(loginInput.Email);
            if (userFounded is null) return null;
            if (userFounded.Pass != loginInput.Pass) return null;

            var expiracion = DateTime.UtcNow.AddMinutes(GetMinutosDeExpiracion());
            var token = GenerateToken(userFounded, expiracion);

            return new LoginResponse(token, userFounded.Email, userFounded.Nombre, userFounded.Apellido, userFounded.Rol, expiracion);
        }

        private int GetMinutosDeExpiracion()
        {
            var minutos = _configuration["Jwt:MinutosDeExpiracion"];
            if (int.TryParse(minutos, out int resultado)) return resultado;

            return 120;
        }

        private string GenerateToken(Usuario usuarioInput, DateTime expiracion)
        {
            var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioInput.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuarioInput.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, $"{usuarioInput.Nombre} {usuarioInput.Apellido}"),
                new Claim(ClaimTypes.Email, usuarioInput.Email),
                new Claim(ClaimTypes.Role, usuarioInput.Rol)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiracion,
                signingCredentials: credenciales);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
