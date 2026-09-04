using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Data.Implementaciones;
using Core.Interfaces;
using Data.Interfaces;

namespace Core.Services
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IEventoRepository _eventoRepository;
        private readonly ISectorRepository _sectorRepository;

        public CompraService(ICompraRepository compraRepository, IEventoRepository eventoRepository, ISectorRepository sectorRepository)
        {
            _compraRepository = compraRepository;
            _eventoRepository = eventoRepository;
            _sectorRepository = sectorRepository;
        }

        public async Task<bool> GenerarCompraAsync(int usuarioId, int eventoId, int sectorId, int cantidad)
        {
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad de entradas debe ser mayor a cero.");
            }
            var evento = await _eventoRepository.GetOneById(eventoId);
            if (evento == null || !evento.EstaCancelado)
            {
                throw new ArgumentException("El evento no existe o ya caduco");
            }
            var sector = await _sectorRepository.ObtenerSectorPorIdAsync(sectorId);
            if (sector == null)
            {
                throw new Exception("El sector seleccionado no existe.");
            }
            int entradasVendidas = await _compraRepository.ObtenerCantidadEntradasVendidasAsync(eventoId, sectorId);
            int capacidadRestante = sector.CapacidadMaxima - entradasVendidas;

            if (cantidad > capacidadRestante)
            {
                throw new Exception("$\"Stock insuficiente. Solo quedan {capacidadRestante} lugares en este sector.");
            }

            decimal montoCalculado = sector.PrecioBase * cantidad;

            var nuevaCompra = new Compra
            {
                UsuarioId = usuarioId,
                //EventoId = eventoId,
                //SectorId = sectorId,
                Cantidad = cantidad,
                MontoTotal = montoCalculado,
                FechaCompra = DateTime.Now,
                Entradas = new List<Entrada>()
            }; 

            for (int i = 0; i < cantidad; i++)
            {
                var nuevaEntrada = new Entrada
                {
                    EventoId = eventoId,
                    SectorId = sectorId,
                };
                nuevaCompra.Entradas.Add(nuevaEntrada);
            }
            await _compraRepository.AddAsync(nuevaCompra);
            // Aquí programaremos la validación de stock y cálculo de precios
            return true;
        }
    }
}
