using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Configuracion;
using Kicket.ApiClient.Sesion;
using Kicket.WinForms.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace Kicket.WinForms
{
    // Punto de entrada de la aplicación de escritorio (solo para administradores).
    internal static class Program
    {
        // Contenedor de dependencias, accesible desde los formularios para abrir otros formularios.
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        // Main: configura la inyección de dependencias y abre el formulario de login.
        [STAThread]
        static void Main()
        {
            // Configuración visual estándar de WinForms (DPI, fuentes, estilos).
            ApplicationConfiguration.Initialize();

            // Crea el contenedor de servicios.
            var services = new ServiceCollection();

            services.AddHttpClient();

            // Registra los clientes HTTP de la API apuntando a la URL donde corre WebApi.
            services.AddKicketApiClient(options =>
            {
                options.BaseUrl = "http://localhost:5268/";
                options.TimeoutSegundos = 30;
            });

            // Una sola sesión para toda la aplicación (Singleton): hay un solo usuario frente a la pantalla.
            services.AddSingleton<ISesionUsuario, SesionUsuario>();

            // Registramos todos los formularios
            services.AddTransient<FormRegistro>();
            services.AddTransient<FormLogin>();
            services.AddTransient<FormPrincipal>();
            services.AddTransient<FormClub>();
            services.AddTransient<FormEstadio>();
            services.AddTransient<FormEvento>();
            services.AddTransient<FormUsuario>();
            services.AddTransient<FormSectorDetalle>();
            services.AddTransient<FormCompras>();
            services.AddTransient<FormTicketQr>();
            services.AddTransient<FormReportes>();

            // Construye el contenedor con todo lo registrado.
            ServiceProvider = services.BuildServiceProvider();

            // Arranca la aplicación mostrando el login.
            Application.Run(ServiceProvider.GetRequiredService<FormLogin>());
        }
    }
}
