using Microsoft.AspNetCore.Components;
using Kicket.ApiClient.Http;

namespace Kicket.Blazor.Core.HTTP
{
    // Clase base para páginas que llaman a la API: centraliza el manejo de errores y el estado "procesando".
    public class PaginaApi : ComponentBase
    {
        // Mensaje de error para mostrar en pantalla (null si no hay error).
        public string? MensajeError { get; set; }
        // true mientras se espera la respuesta de la API.
        public bool Procesando { get; set; }

        // Ejecuta una acción contra la API atrapando los errores y traduciéndolos a un mensaje.
        public async Task SecureExec(Func<Task> Action)
        {
            try
            {
                // Limpia el error anterior y marca que está procesando.
                MensajeError = null;
                Procesando = true;
                await Action();
            }
            // Errores que vienen de la API: elige el mensaje según el tipo.
            catch (ApiException exe)
            {
                if (exe.EsFalloDeConexion)
                {
                    MensajeError = "Ocurrio un error al conectar con el servidor, Intente de nuevo mas tarde";
                }
                else if (exe.NoEncontrado)
                {
                    MensajeError = "Recurso no encontrado";
                }
                else if (exe.NoAutenticado)
                {
                    MensajeError = exe.MensajeCompleto();
                }
                else if (exe.SinPermisos)
                {
                    MensajeError = "No tienes Permiso para realizar esta Accion";
                }
                else if (exe.HayErroresDeValidacion)
                {
                    MensajeError = "Los datos enviados no son validos";
                }
                else
                {
                    MensajeError = exe.MensajeCompleto();
                }
            }
            // Cualquier otro error inesperado.
            catch (Exception)
            {
                MensajeError = "Ocurrio un error inesperado";
            }
            // Siempre: deja de procesar y redibuja la pantalla.
            finally
            {
                Procesando = false;
                StateHasChanged();
            }
        }
    }
}
