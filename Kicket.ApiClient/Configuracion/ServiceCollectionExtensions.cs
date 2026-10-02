using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Clientes;
using Kicket.ApiClient.Http;
using Kicket.ApiClient.Sesion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Kicket.ApiClient.Configuracion
{
    /// <summary>
    /// Punto de entrada de la capa cliente. El proyecto que la consume solo llama a
    /// AddKicketApiClient(...) y ya tiene inyectables IClubApiClient, IAuthApiClient, etc.
    ///
    /// La capa la comparten dos interfaces con necesidades distintas, y el registro esta
    /// armado para servir a las dos sin que ninguna tenga que configurar nada aparte:
    ///
    /// - WinForms: un solo usuario por proceso, asi que la sesion por defecto es singleton.
    /// - Blazor Server: un usuario por circuito. Ahi la sesion NO puede ser singleton, porque
    ///   seria la misma para todos los que abran la pagina: se loguea uno y quedan logueados
    ///   todos. Por eso el host registra su propia ISesionUsuario (con scope) antes de llamar
    ///   aca, y este metodo respeta ese registro en vez de pisarlo.
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

            // TryAdd y no Add: si el host ya registro su propia sesion (el caso de Blazor con
            // BlazorSesionUsuario), gana la suya. Si no registro ninguna (el caso de WinForms),
            // queda esta y la aplicacion funciona sin tener que acordarse de agregarla.
            services.TryAddSingleton<ISesionUsuario, SesionUsuario>();

            services.TryAddSingleton<TransporteCompartido>();
            services.TryAddTransient<AuthTokenHandler>();

            services.AgregarCliente<IClubApiClient, ClubApiClient>(opciones);
            services.AgregarCliente<IEstadioApiClient, EstadioApiClient>(opciones);
            services.AgregarCliente<IUsuarioApiClient, UsuarioApiClient>(opciones);
            services.AgregarCliente<IAuthApiClient, AuthApiClient>(opciones);
            services.AgregarCliente<IEventoApiClient, EventoApiClient>(opciones);
            services.AgregarCliente<ISectorApiClient, SectorApiClient>(opciones);
            services.AgregarCliente<ICompraApiClient, CompraApiClient>(opciones);
            services.AgregarCliente<IEntradaApiClient, EntradaApiClient>(opciones);

            return services;
        }

        /// <summary>
        /// Registra un cliente con scope. No se usa AddHttpClient porque los handlers que
        /// arma IHttpClientFactory viven en un scope propio y se reusan por varios minutos:
        /// con una sesion por usuario, AuthTokenHandler terminaria mandando el token de otro
        /// o uno ya vencido. Armando el HttpClient dentro del scope, el handler recibe la
        /// sesion que corresponde, y el pool de conexiones igual se comparte via
        /// TransporteCompartido.
        /// </summary>
        private static void AgregarCliente<TInterfaz, TImpl>(
            this IServiceCollection services, KicketApiOptions opciones)
            where TInterfaz : class
            where TImpl : class, TInterfaz
        {
            services.AddScoped<TInterfaz>(sp =>
            {
                var conToken = sp.GetRequiredService<AuthTokenHandler>();
                conToken.InnerHandler = sp.GetRequiredService<TransporteCompartido>();

                // disposeHandler: false porque la cadena de handlers no es de este HttpClient:
                // del AuthTokenHandler se ocupa el contenedor y el transporte es compartido.
                var http = new HttpClient(conToken, disposeHandler: false)
                {
                    BaseAddress = new Uri(opciones.BaseUrl),
                    Timeout = TimeSpan.FromSeconds(opciones.TimeoutSegundos)
                };

                return ActivatorUtilities.CreateInstance<TImpl>(sp, http);
            });
        }
    }
}
