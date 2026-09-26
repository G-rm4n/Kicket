using Microsoft.AspNetCore.Components;
using Kicket.ApiClient.Http;

namespace Kicket.Blazor.Core.HTTP
{
    public class PaginaApi:ComponentBase
    {
        public string? MensajeError { get; set; }
        public bool Procesando { get; set; }

        public async Task SecureExec(Func<Task> Action)
        {
            try
            {
                MensajeError = null;
                Procesando = true;
                await Action();
            }
            catch (ApiException exe)
            {
                if (exe.EsFalloDeConexion)
                {
                    MensajeError = "Ocurrio un error al conectar con el servidor, Intente denuevo Mas Tarde";
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
            catch(Exception)
            {
                MensajeError = "Ocurrio un error inesperado";
            }
            finally
            {
                Procesando &= Procesando;
                StateHasChanged();

            }

        }
    }
}
