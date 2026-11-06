using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Clientes;
using Kicket.ApiClient.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Kicket.ApiClient.Configuracion
{
    /// <summary>
    /// Punto de entrada de la capa cliente. El proyecto de escritorio y el web solo llaman a
    /// AddKicketApiClient(...) y ya tienen inyectables IClubApiClient, IAuthApiClient,
    /// IEventoApiClient, ICompraApiClient, IEntradaApiClient, IPagoApiClient, etc.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>Registra la capa cliente leyendo la seccion "KicketApi" de appsettings.json.</summary>
        public static IServiceCollection AddKicketApiClient(
            this IServiceCollection services, IConfiguration configuration)
        {
            // Crea las opciones y las completa con los valores de appsettings.
            var opciones = new KicketApiOptions();
            configuration.GetSection(KicketApiOptions.SeccionConfig).Bind(opciones);

            return services.AddKicketApiClient(opciones);
        }

        /// <summary>Registra la capa cliente con configuracion armada a mano.</summary>
        public static IServiceCollection AddKicketApiClient(
            this IServiceCollection services, Action<KicketApiOptions> configurar)
        {
            // Crea las opciones y deja que el que llama las complete.
            var opciones = new KicketApiOptions();
            configurar(opciones);

            return services.AddKicketApiClient(opciones);
        }

        // Registro real: valida la URL, registra opciones, el handler del token y todos los clientes.
        private static IServiceCollection AddKicketApiClient(
            this IServiceCollection services, KicketApiOptions opciones)
        {
            // Sin URL base no se puede hablar con la API.
            if (string.IsNullOrWhiteSpace(opciones.BaseUrl))
                throw new InvalidOperationException(
                    "Falta configurar KicketApi:BaseUrl con la URL de la API.");

            // HttpClient resuelve las rutas relativas solo si la base termina en barra.
            if (!opciones.BaseUrl.EndsWith('/'))
                opciones.BaseUrl += "/";

            // Las opciones se registran como Singleton (una sola instancia para toda la app).
            services.AddSingleton(opciones);

            // El handler que agrega el token se registra como Transient (uno nuevo cada vez que se pide).
            services.AddTransient<AuthTokenHandler>();

            // Registra cada cliente tipado con su interfaz.
            services.AgregarCliente<IClubApiClient, ClubApiClient>(opciones);
            services.AgregarCliente<IEstadioApiClient, EstadioApiClient>(opciones);
            services.AgregarCliente<ISectorApiClient, SectorApiClient>(opciones);
            services.AgregarCliente<IUsuarioApiClient, UsuarioApiClient>(opciones);
            services.AgregarCliente<IAuthApiClient, AuthApiClient>(opciones);
            services.AgregarCliente<IEventoApiClient, EventoApiClient>(opciones);
            services.AgregarCliente<ICompraApiClient, CompraApiClient>(opciones);
            services.AgregarCliente<IEntradaApiClient, EntradaApiClient>(opciones);
            services.AgregarCliente<IPagoApiClient, PagoApiClient>(opciones);
            services.AgregarCliente<IReporteApiClient, ReporteApiClient>(opciones);

            return services;
        }

        // Registra un cliente como Scoped: le arma un HttpClient con el AuthTokenHandler,
        // la URL base y el timeout, y crea la instancia del cliente pasándole ese HttpClient.
        private static void AgregarCliente<TInterfaz, TImpl>(
            this IServiceCollection services, KicketApiOptions opciones)
            where TInterfaz : class
            where TImpl : class, TInterfaz
        {
            services.AddScoped<TInterfaz, TImpl>(sp =>
            {
                // Pide el handler del token y le encadena el handler HTTP real.
                var handler = sp.GetRequiredService<AuthTokenHandler>();

                handler.InnerHandler = new HttpClientHandler();

                // HttpClient con la URL base y el timeout configurados.
                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(opciones.BaseUrl),
                    Timeout = TimeSpan.FromSeconds(opciones.TimeoutSegundos)
                };

                // Crea el cliente concreto resolviendo el resto de sus dependencias desde el contenedor.
                return ActivatorUtilities.CreateInstance<TImpl>(sp, httpClient);
            });
        }
    }
}
