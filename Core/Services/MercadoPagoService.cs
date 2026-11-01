using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Data.Interfaces;
using Kicket.Contracts.Pagos;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    // Servicio de pagos: crea la preferencia en Mercado Pago o, si no hay token válido, usa un simulador local.
    public class MercadoPagoService : IMercadoPagoService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventoRepository _eventoRepository;
        private readonly ISectorRepository _sectorRepository;
        private readonly ICompraRepository _compraRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<MercadoPagoService> _logger;

        // Recibe la configuración, los repositorios y el logger por inyección de dependencias.
        public MercadoPagoService(
            IConfiguration configuration,
            IEventoRepository eventoRepository,
            ISectorRepository sectorRepository,
            ICompraRepository compraRepository,
            IUsuarioRepository usuarioRepository,
            ILogger<MercadoPagoService> logger)
        {
            _configuration = configuration;
            _eventoRepository = eventoRepository;
            _sectorRepository = sectorRepository;
            _compraRepository = compraRepository;
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        // Crea una preferencia de pago para comprar entradas.
        // Hace las mismas validaciones que la compra, calcula el monto y decide entre Mercado Pago real o simulador.
        public async Task<PreferenciaPagoResponse> CrearPreferenciaAsync(PreferenciaPagoRequest request, string baseUrlRetorno)
        {
            // Validación: cantidad mayor a cero.
            if (request.Cantidad <= 0)
            {
                throw new ArgumentException("La cantidad de entradas debe ser mayor a cero.");
            }

            // Validación: el evento existe y está disponible.
            var evento = await _eventoRepository.GetOneById(request.EventoId);
            if (evento == null || !evento.EstaDisponible())
            {
                throw new ArgumentException("El evento seleccionado no existe o no se encuentra disponible.");
            }

            // Validación: el sector existe y es del mismo estadio que el evento.
            var sector = await _sectorRepository.ObtenerSectorPorIdAsync(request.SectorId);
            if (sector == null)
            {
                throw new ArgumentException("El sector seleccionado no existe.");
            }

            if (sector.EstadioId != evento.EstadioId)
            {
                throw new ArgumentException("El sector seleccionado no pertenece al estadio donde se disputa el evento.");
            }

            // Validación de stock disponible en el sector.
            int entradasVendidas = await _compraRepository.ObtenerCantidadEntradasVendidasAsync(request.EventoId, request.SectorId);
            int capacidadRestante = sector.CapacidadMaxima - entradasVendidas;

            if (request.Cantidad > capacidadRestante)
            {
                throw new ArgumentException($"Stock insuficiente. Solo quedan {capacidadRestante} lugares en este sector.");
            }

            // Calcula el total y lee el Access Token de Mercado Pago desde la configuración.
            decimal montoTotal = sector.PrecioBase * request.Cantidad;
            string token = _configuration["MercadoPago:AccessToken"] ?? string.Empty;

            // El token es válido solo si empieza con "TEST-" y no es el texto de ejemplo del appsettings.
            bool tokenValido = !string.IsNullOrWhiteSpace(token)
                               && token.StartsWith("TEST-", StringComparison.OrdinalIgnoreCase)
                               && !token.Contains("TU_ACCESS_TOKEN", StringComparison.OrdinalIgnoreCase)
                               && !token.Contains("PROPORCIONAR", StringComparison.OrdinalIgnoreCase);

            // Con token válido intenta crear la preferencia real con el SDK de Mercado Pago.
            if (tokenValido)
            {
                try
                {
                    MercadoPagoConfig.AccessToken = token;

                    var client = new PreferenceClient();

                    // Parámetros que vuelven en la URL de retorno para poder generar la compra después.
                    string baseClean = baseUrlRetorno.TrimEnd('/');
                    string callbackParams = $"eventoId={request.EventoId}&sectorId={request.SectorId}&cantidad={request.Cantidad}&usuarioId={request.UsuarioId}";

                    // Arma la preferencia: el ítem a cobrar, las URLs de retorno (aprobado/rechazado/pendiente) y una referencia externa.
                    var preferenceRequest = new PreferenceRequest
                    {
                        Items = new List<PreferenceItemRequest>
                        {
                            new PreferenceItemRequest
                            {
                                Title = $"Entrada: {evento.Nombre} - {sector.Nombre}",
                                Description = $"{request.Cantidad} entrada(s) para {evento.Nombre}",
                                Quantity = request.Cantidad,
                                CurrencyId = "ARS",
                                UnitPrice = sector.PrecioBase
                            }
                        },
                        BackUrls = new PreferenceBackUrlsRequest
                        {
                            Success = $"{baseClean}/pago/resultado?status=approved&{callbackParams}",
                            Failure = $"{baseClean}/pago/resultado?status=rejected&{callbackParams}",
                            Pending = $"{baseClean}/pago/resultado?status=pending&{callbackParams}"
                        },
                        AutoReturn = "approved",
                        ExternalReference = $"KCKT_{request.UsuarioId}_{request.EventoId}_{request.SectorId}_{request.Cantidad}_{Guid.NewGuid():N}",
                        BinaryMode = true
                    };

                    // Llama a la API de Mercado Pago y devuelve la URL de pago (sandbox si existe).
                    var preference = await client.CreateAsync(preferenceRequest);

                    return new PreferenciaPagoResponse
                    {
                        PreferenceId = preference.Id,
                        RedirectUrl = preference.SandboxInitPoint ?? preference.InitPoint,
                        MontoTotal = montoTotal,
                        EsModoSimulado = false,
                        EventoNombre = evento.Nombre,
                        SectorNombre = sector.Nombre,
                        Cantidad = request.Cantidad,
                        UsuarioId = request.UsuarioId,
                        EventoId = request.EventoId,
                        SectorId = request.SectorId
                    };
                }
                // Si falla la API real, lo registra en el log y sigue con el simulador.
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "No se pudo crear la preferencia con la API oficial de Mercado Pago. Se activa el Simulador Sandbox de respaldo.");
                }
            }

            // Modo Simulador Sandbox Local de Mercado Pago
            // Genera un id de preferencia simulado y la URL de la página del simulador con todos los datos.
            string simPrefId = $"SIM-MP-{Guid.NewGuid():N}".ToUpperInvariant();
            string simRedirect = $"/pago/simulador?prefId={simPrefId}&usuarioId={request.UsuarioId}&eventoId={request.EventoId}&sectorId={request.SectorId}&cantidad={request.Cantidad}&monto={montoTotal}&eventoNombre={Uri.EscapeDataString(evento.Nombre)}&sectorNombre={Uri.EscapeDataString(sector.Nombre)}";

            return new PreferenciaPagoResponse
            {
                PreferenceId = simPrefId,
                RedirectUrl = simRedirect,
                MontoTotal = montoTotal,
                EsModoSimulado = true,
                EventoNombre = evento.Nombre,
                SectorNombre = sector.Nombre,
                Cantidad = request.Cantidad,
                UsuarioId = request.UsuarioId,
                EventoId = request.EventoId,
                SectorId = request.SectorId
            };
        }

        // Decide si un pago está aprobado.
        public async Task<bool> ValidarPagoAprobadoAsync(string paymentId)
        {
            // Sin id de pago no hay aprobación.
            if (string.IsNullOrWhiteSpace(paymentId))
            {
                return false;
            }

            // Pagos generados por el simulador o entorno de prueba
            if (paymentId.StartsWith("SIM-", StringComparison.OrdinalIgnoreCase)
                || paymentId.StartsWith("TEST-", StringComparison.OrdinalIgnoreCase)
                || paymentId.StartsWith("MP-", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Si es un ID numérico real de Mercado Pago y hay token configurado
            // Con id numérico y token, consulta el estado real a Mercado Pago; si la consulta falla lo da por válido.
            string token = _configuration["MercadoPago:AccessToken"] ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(token) && long.TryParse(paymentId, out long numericId))
            {
                try
                {
                    MercadoPagoConfig.AccessToken = token;
                    var client = new MercadoPago.Client.Payment.PaymentClient();
                    var payment = await client.GetAsync(numericId);
                    return payment.Status == "approved";
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error al consultar estado del pago en Mercado Pago para ID {PaymentId}. Asumiendo válido en Sandbox.", paymentId);
                    return true;
                }
            }

            // En cualquier otro caso lo da por aprobado (validación permisiva, pensada para pruebas).
            return true;
        }
    }
}
