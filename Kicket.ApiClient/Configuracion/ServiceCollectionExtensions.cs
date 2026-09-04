using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Clientes;
using Kicket.ApiClient.Http;
using Kicket.ApiClient.Sesion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kicket.ApiClient.Configuracion
{
    /// <summary>
    /// Punto de entrada de la capa cliente. El proyecto de escritorio solo llama a
    /// AddKicketApiClient(...) y ya tiene inyectables IClubApiClient, IAuthApiClient, etc.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>Registra la capa cliente leyendo la seccion "KicketApi" de appsettings.json.</summary>
        public static IServiceCollection AddKicketApiClient(
            this IServiceCollection services, IConfiguration configuration)
        {
            var opciones = new KicketApiOptions();
            configuration.GetSection(KicketApiOptions.SeccionConfig).Bind(opciones);

            return services.AddKicketApiClient(opciones);
        }

        /// <summary>Registra la capa cliente con configuracion armada a mano.</summary>
        public static IServiceCollection AddKicketApiClient(
            this IServiceCollection services, Action<KicketApiOptions> configurar)
        {
            var opciones = new KicketApiOptions();
            configurar(opciones);

            return services.AddKicketApiClient(opciones);
        }

        private static IServiceCollection AddKicketApiClient(
            this IServiceCollection services, KicketApiOptions opciones)
        {
            if (string.IsNullOrWhiteSpace(opciones.BaseUrl))
                throw new InvalidOperationException(
                    "Falta configurar KicketApi:BaseUrl con la URL de la API.");

            // HttpClient resuelve las rutas relativas solo si la base termina en barra.
            if (!opciones.BaseUrl.EndsWith('/'))
                opciones.BaseUrl += "/";

            services.AddSingleton(opciones);

            // Una sola sesion por proceso: es el usuario sentado frente a la aplicacion.
            services.AddSingleton<ISesionUsuario, SesionUsuario>();
            services.AddTransient<AuthTokenHandler>();

            services.AgregarCliente<IClubApiClient, ClubApiClient>(opciones);
            services.AgregarCliente<IEstadioApiClient, EstadioApiClient>(opciones);
            services.AgregarCliente<IUsuarioApiClient, UsuarioApiClient>(opciones);
            services.AgregarCliente<IAuthApiClient, AuthApiClient>(opciones);

            return services;
        }

        private static void AgregarCliente<TInterfaz, TImpl>(
            this IServiceCollection services, KicketApiOptions opciones)
            where TInterfaz : class
            where TImpl : class, TInterfaz
        {
            services.AddHttpClient<TInterfaz, TImpl>(http =>
                   {
                       http.BaseAddress = new Uri(opciones.BaseUrl);
                       http.Timeout = TimeSpan.FromSeconds(opciones.TimeoutSegundos);
                   })
                   .AddHttpMessageHandler<AuthTokenHandler>();
        }
    }
}
