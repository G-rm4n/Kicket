
namespace Kiketpropio.Dominio.DTOs
{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Pass { get; set; }

        public LoginRequest()
        {
            Email = string.Empty;
            Pass = string.Empty;
        }

        public LoginRequest(string email, string pass)
        {
            Email = email;
            Pass = pass;
        }
    }
}
