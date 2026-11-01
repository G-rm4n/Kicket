using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Core.Interfaces;
using Data.Interfaces;

namespace Core.Services
{
    // Lógica de negocio de compras: valida el pedido, controla el stock y genera las entradas.
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly ISectorRepository _sectorRepository;

        // Recibe los repositorios de compras, eventos y sectores por inyección de dependencias.
        public CompraService(ICompraRepository compraRepository, IEventoRepository eventoRepository, ISectorRepository sectorRepository)
        {
            _compraRepository = compraRepository;
            _eventoRepository = eventoRepository;
            _sectorRepository = sectorRepository;
        }

        // Genera una compra completa a partir del usuario, evento, sector y cantidad.
        public async Task<Compra> GenerarCompraAsync(
            int usuarioId,
            int eventoId,
            int sectorId,
            int cantidad,
            string? metodoPago = "MercadoPago",
            string? paymentId = null,
            string? estadoPago = "Aprobado")
        {
            // 1) La cantidad tiene que ser mayor a cero.
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad de entradas debe ser mayor a cero.");
            }
            // 2) El evento tiene que existir y estar disponible (no cancelado y con fecha futura).
            var evento = await _eventoRepository.GetOneById(eventoId);
            if (evento == null || !evento.EstaDisponible())
            {
                throw new ArgumentException("El evento no existe o ya caduco.");
            }
            // 3) El sector tiene que existir...
            var sector = await _sectorRepository.ObtenerSectorPorIdAsync(sectorId);
            if (sector == null)
            {
                throw new ArgumentException("El sector seleccionado no existe.");
            }
            // ...y pertenecer al mismo estadio que el evento.
            if (sector.EstadioId != evento.EstadioId)
            {
                throw new ArgumentException("El sector seleccionado no pertenece al estadio donde se disputa el evento.");
            }

            // 4) Control de stock: capacidad del sector menos las entradas ya vendidas.
            int entradasVendidas = await _compraRepository.ObtenerCantidadEntradasVendidasAsync(eventoId, sectorId);
            int capacidadRestante = sector.CapacidadMaxima - entradasVendidas;

            if (cantidad > capacidadRestante)
            {
                throw new ArgumentException($"Stock insuficiente. Solo quedan {capacidadRestante} lugares en este sector.");
            }

            // 5) El monto se calcula en el servidor: precio del sector por cantidad.
            decimal montoCalculado = sector.PrecioBase * cantidad;

            // 6) Arma la compra con los datos del pago.
            var nuevaCompra = new Compra
            {
                UsuarioId = usuarioId,
                Cantidad = cantidad,
                MontoTotal = montoCalculado,
                FechaCompra = DateTime.Now,
                MetodoPago = metodoPago ?? "MercadoPago",
                PaymentId = paymentId,
                EstadoPago = estadoPago ?? "Aprobado",
                Entradas = new List<Entrada>()
            };

            // La numeración es automática SOLAMENTE si el sector seleccionado es en Platea.
            // Si el sector es Popular, no lleva numeración (FilaAsiento = null).
            bool esPlatea = sector.Nombre.Contains("Platea", StringComparison.OrdinalIgnoreCase);

            // Si es platea, trae las ubicaciones ya ocupadas para no repetirlas.
            HashSet<string>? ubicacionesOcupadas = null;
            if (esPlatea)
            {
                var ocupadas = await _compraRepository.ObtenerUbicacionesOcupadasAsync(eventoId, sectorId);
                ubicacionesOcupadas = new HashSet<string>(ocupadas, StringComparer.OrdinalIgnoreCase);
            }

            // Recorre los asientos en orden: 20 por fila (índice 1 → Fila 1 Asiento 1, índice 21 → Fila 2 Asiento 1).
            int siguienteIndiceAsiento = 1;
            const int asientosPorFila = 20;

            // 7) Crea una entrada por cada unidad comprada.
            for (int i = 0; i < cantidad; i++)
            {
                string? filaAsiento = null;

                // Busca el primer asiento libre: si HashSet.Add devuelve true, el asiento no estaba ocupado.
                if (esPlatea && ubicacionesOcupadas is not null)
                {
                    while (true)
                    {
                        int fila = ((siguienteIndiceAsiento - 1) / asientosPorFila) + 1;
                        int asiento = ((siguienteIndiceAsiento - 1) % asientosPorFila) + 1;
                        string candidato = $"Fila {fila}, Asiento {asiento}";
                        siguienteIndiceAsiento++;

                        if (ubicacionesOcupadas.Add(candidato))
                        {
                            filaAsiento = candidato;
                            break;
                        }
                    }
                }

                // Cada entrada lleva un código QR único "KCKT-" + GUID en mayúsculas.
                var nuevaEntrada = new Entrada
                {
                    EventoId = eventoId,
                    SectorId = sectorId,
                    FilaAsiento = filaAsiento,
                    CodigoQr = $"KCKT-{Guid.NewGuid():N}".ToUpperInvariant()
                };
                nuevaCompra.Entradas.Add(nuevaEntrada);
            }

            // 8) Guarda la compra con todas sus entradas en una sola operación.
            await _compraRepository.AddAsync(nuevaCompra);
            return nuevaCompra;
        }

        // Devuelve una compra por id.
        public async Task<Compra?> ObtenerPorIdAsync(int id)
        {
            return await _compraRepository.GetByIdAsync(id);
        }

        // Devuelve todas las compras.
        public async Task<IEnumerable<Compra>> ObtenerTodosAsync()
        {
            return await _compraRepository.GetAllAsync();
        }

        // Elimina una compra (y sus entradas).
        public async Task<bool> EliminarCompraAsync(int id)
        {
            return await _compraRepository.DeleteAsync(id);
        }
    }
}
