using Kicket.Blazor.Core.state;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Threading.Tasks;
using Kicket.Blazor.Core.HTTP;

namespace Kicket.Blazor.Core.Auth
{
    // Clase base para páginas que solo pueden ver usuarios logueados con ciertos roles (por defecto Admin).
    public class PaginaProtegida:PaginaApi
    {
        // Proveedor del estado de autenticación (inyectado).
        [Inject]
        protected KicketAuthStateProvider stateProvider { get; set; } = null!;

        // Para redirigir a otra página (inyectado).
        [Inject]
        protected NavigationManager navigationManager { get; set; } = null!;

        // Roles que pueden entrar a la página; cada página puede cambiarlos.
        protected IEnumerable<string> RolesPermitidos { get; set; } = ["Admin"];

        // Al iniciar la página, controla login y rol.
        protected override async Task OnInitializedAsync()
        {
            var EstaAutenticado = await stateProvider.GetAuthenticationStateAsync();

            // Si no está logueado, lo manda al login.
            if (EstaAutenticado.User.Identity?.IsAuthenticated != true)
            {
                navigationManager.NavigateTo("/Auth/Login");
                return;
            }

            // Si está logueado pero no tiene un rol permitido, lo manda al inicio.
            if (!RolesPermitidos.Any(r => EstaAutenticado.User.IsInRole(r)))
            {
                navigationManager.NavigateTo("/");
                return;
            }
        }
    }
}
