using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.Contracts.Auth
{
    // Respuesta del registro: solo un mensaje de confirmación para mostrar al usuario.
    public class RegisterResponse
    {
        // Mensaje que devuelve la API (ej. "Usuario registrado correctamente").
        public string Mensaje { get; set; } = string.Empty;
    }
}
