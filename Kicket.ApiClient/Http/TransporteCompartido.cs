namespace Kicket.ApiClient.Http
{
    /// <summary>
    /// Transporte HTTP unico para toda la aplicacion. Los clientes se registran por scope
    /// (lo necesita Blazor, donde la sesion es una por usuario conectado), pero el pool de
    /// conexiones tiene que ser uno solo: si cada cliente armara su propio HttpClientHandler,
    /// cada pantalla abriria sockets nuevos y no se reusarian nunca.
    ///
    /// Es el mismo reparto que hace IHttpClientFactory: muchos HttpClient baratos adelante,
    /// un transporte caro y compartido atras.
    /// </summary>
    public sealed class TransporteCompartido : DelegatingHandler
    {
        public TransporteCompartido()
            : base(new SocketsHttpHandler
            {
                // Las conexiones se reciclan cada tanto para que un cambio de DNS no quede
                // cacheado para siempre: es el problema clasico de dejar un HttpClient eterno.
                PooledConnectionLifetime = TimeSpan.FromMinutes(5)
            })
        {
        }

        /// <summary>
        /// A proposito no hace nada. Cuando el contenedor cierra un scope dispone el
        /// AuthTokenHandler de ese scope, y un DelegatingHandler arrastra en el Dispose a su
        /// InnerHandler: sin este corte, el primer scope en cerrarse se llevaria puesto el
        /// transporte que comparten todos los demas.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
        }
    }
}
