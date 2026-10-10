using Kicket.Blazor.Core.state;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Threading.Tasks;
using Kicket.Blazor.Core.HTTP;

namespace Kicket.Blazor.Core.Auth
{
    public class PaginaProtegida:PaginaApi
    {
        [Inject]
        protected KicketAuthStateProvider stateProvider { get; set; } = null!;

        [Inject]
        protected NavigationManager navigationManager { get; set; } = null!;

        protected IEnumerable<string> RolesPermitidos { get; set; } = ["Admin"];

        protected override async Task OnInitializedAsync()
        {
            var EstaAutenticado = await stateProvider.GetAuthenticationStateAsync();

            if (EstaAutenticado.User.Identity?.IsAuthenticated != true)
            {
                navigationManager.NavigateTo("/Auth/Login");
                return;
            }

            if (!RolesPermitidos.Any(r => EstaAutenticado.User.IsInRole(r)))
            {
                navigationManager.NavigateTo("/");
                return;
            }
        }
    }
}
