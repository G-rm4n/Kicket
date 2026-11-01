using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Core.Interfaces
{
    // Contrato del servicio de autenticación (login con JWT y registro).
    public interface IAuthService
    {
        // Valida email y contraseña; si son correctos devuelve el token JWT, su vencimiento y el usuario. Si no, null.
        Task<(string Token, DateTime ExpiraEn, Usuario Usuario)?> Login(string mail, string pass);
        // Registra un usuario nuevo (valida email repetido y hashea la contraseña).
        Task RegistrarUsuario(Usuario usuario);
    }
}