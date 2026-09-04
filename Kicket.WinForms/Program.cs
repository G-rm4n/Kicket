using Kicket.ApiClient.Configuracion;
using Kicket.WinForms.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Kicket.WinForms
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();

            services.AddKicketApiClient(options =>
            {
                options.BaseUrl = "https://localhost:7255/";
                options.TimeoutSegundos = 30;
            });

            services.AddTransient<FormLogin>();
            services.AddTransient<FormPrincipal>();

            using var serviceProvider = services.BuildServiceProvider();

            Application.Run(
                serviceProvider.GetRequiredService<FormLogin>()
            );
        }
    }
}