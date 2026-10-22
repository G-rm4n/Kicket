using Kiketpropio.Dominio.DTOs;

namespace Kiketpropio.Services.Interfaces
{
    public interface IAuthService
    {
        LoginResponse? Login(LoginRequest loginInput);
    }
}
