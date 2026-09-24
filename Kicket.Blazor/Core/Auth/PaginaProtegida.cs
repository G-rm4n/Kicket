using Kicket.Blazor.Core.state;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Kicket.Blazor.Core.Auth
{
    public class PaginaProtegida:ComponentBase
    {
        [Inject]
        protected KicketAuthStateProvider stateProvider { get; set; } = null!;

        [Inject]
        protected NavigationManager navigationManager { get; set; } = null!;

        protected override async void OnInitialized()
        {
            var EstaAutenticado = await stateProvider.GetAuthenticationStateAsync();

            if(EstaAutenticado.User.Identity?.IsAuthenticated != true)
            {
                navigationManager.NavigateTo("/Auth/Login");
            }
        }
    }
}
