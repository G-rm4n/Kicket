using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Clientes;
using Kicket.ApiClient.Configuracion;
using Kicket.ApiClient.Sesion;
using Kicket.WinForms.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace Kicket.WinForms
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();

            services.AddHttpClient();

            // ¡Mantenemos la configuración vital de tu compañero!
            services.AddKicketApiClient(options =>
            {
                options.BaseUrl = "http://localhost:5268/";
                options.TimeoutSegundos = 30;
            });

            services.AddSingleton<ISesionUsuario, SesionUsuario>();

            // Registramos todos los formularios
            services.AddTransient<FormRegistro>();
            services.AddTransient<FormLogin>();
            services.AddTransient<FormPrincipal>();
            services.AddTransient<FormClub>();
            services.AddTransient<FormEstadio>();
            services.AddTransient<FormUsuario>();

            // Guardamos el proveedor en la variable estática
            ServiceProvider = services.BuildServiceProvider();

            
            Application.Run(ServiceProvider.GetRequiredService<FormLogin>());
        }
    }
}