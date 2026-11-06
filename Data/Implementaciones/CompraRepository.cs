using Data.Consultas;
using Data.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Compras;
using Kicket.Contracts.Entradas;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Implementaciones
{
    // Implementación del repositorio de compras usando EF Core.
    public class CompraRepository : ICompraRepository
    {
        private readonly TPIContext context;

        // Recibe el DbContext por inyección de dependencias.
        public CompraRepository(TPIContext context)
        {
            this.context = context;
        }

        // Trae todas las compras con sus entradas, de la más nueva a la más vieja.
        public async Task<IEnumerable<Compra>> GetAllAsync()
        {
            return await context.Compras
                .Include(c => c.Entradas)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        // Compras de un usuario, filtradas en la base (antes se traían todas y se filtraba en memoria).
        public async Task<IEnumerable<Compra>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await context.Compras
                .Include(c => c.Entradas)
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        // Busca una compra por id incluyendo sus entradas.
        public async Task<Compra?> GetByIdAsync(int id)
        {
            return await context.Compras
                .Include(c => c.Entradas)
                .FirstOrDefaultAsync(c => c.CompraId == id);
        }

        // Busca la compra con sus entradas; borra primero las entradas y después la compra.
        // Devuelve false si la compra no existe.
        public async Task<bool> DeleteAsync(int id)
        {
            var compraFound = await context.Compras
                .Include(c => c.Entradas)
                .FirstOrDefaultAsync(e => e.CompraId == id);

            if (compraFound is not null)
            {
                if (compraFound.Entradas.Any())
                {
                    context.Entradas.RemoveRange(compraFound.Entradas);
                }
                context.Compras.Remove(compraFound);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Guarda la compra (EF también inserta las entradas de su lista).
        public async Task AddAsync(Compra compra)
        {
            context.Compras.Add(compra);
            await context.SaveChangesAsync();
        }

        // Cuenta las entradas ya vendidas para ese evento y sector (sirve para controlar el cupo).
        public async Task<int> ObtenerCantidadEntradasVendidasAsync(int eventoId, int sectorId)
        {
            return await context.Entradas
                .CountAsync(e => e.EventoId == eventoId && e.SectorId == sectorId);
        }

        // Devuelve los textos "Fila X, Asiento Y" ocupados en ese evento y sector (para no repetir asientos).
        public async Task<IReadOnlyList<string>> ObtenerUbicacionesOcupadasAsync(int eventoId, int sectorId)
        {
            return await context.Entradas
                .Where(e => e.EventoId == eventoId && e.SectorId == sectorId && e.FilaAsiento != null)
                .Select(e => e.FilaAsiento!)
                .ToListAsync();
        }

        // Pagina de compras (administración): filtra, ordena y pagina en la base; después resuelve
        // los nombres de usuario, evento y sector solo de las compras que quedaron en la página.
        public async Task<PagedResult<CompraDto>> ObtenerPaginadoAsync(CompraFiltro filtro, CancellationToken ct = default)
        {
            IQueryable<Compra> consulta = context.Compras.AsNoTracking().Include(c => c.Entradas);

            if (filtro.CompraId.HasValue)
            {
                int compraId = filtro.CompraId.Value;
                consulta = consulta.Where(c => c.CompraId == compraId);
            }
            if (filtro.UsuarioId.HasValue)
            {
                int usuarioId = filtro.UsuarioId.Value;
                consulta = consulta.Where(c => c.UsuarioId == usuarioId);
            }
            if (filtro.EventoId.HasValue)
            {
                int eventoId = filtro.EventoId.Value;
                consulta = consulta.Where(c => c.Entradas.Any(e => e.EventoId == eventoId));
            }
            if (filtro.Desde.HasValue)
            {
                var desde = filtro.Desde.Value.Date;
                consulta = consulta.Where(c => c.FechaCompra >= desde);
            }
            if (filtro.Hasta.HasValue)
            {
                var hastaExclusivo = filtro.Hasta.Value.Date.AddDays(1);
                consulta = consulta.Where(c => c.FechaCompra < hastaExclusivo);
            }
            if (filtro.PartidoDesde.HasValue)
            {
                var desde = filtro.PartidoDesde.Value.Date;
                consulta = consulta.Where(c => c.Entradas.Any(e => context.Eventos.Any(ev => ev.IdEvento == e.EventoId && ev.Fecha >= desde)));
            }
            if (filtro.PartidoHasta.HasValue)
            {
                var hastaExclusivo = filtro.PartidoHasta.Value.Date.AddDays(1);
                consulta = consulta.Where(c => c.Entradas.Any(e => context.Eventos.Any(ev => ev.IdEvento == e.EventoId && ev.Fecha < hastaExclusivo)));
            }
            if (!string.IsNullOrWhiteSpace(filtro.EstadoPago))
            {
                var estado = filtro.EstadoPago.Trim();
                consulta = consulta.Where(c => c.EstadoPago == estado);
            }
            if (filtro.Busqueda is not null)
            {
                var texto = filtro.Busqueda;
                consulta = consulta.Where(c => (c.PaymentId != null && c.PaymentId.Contains(texto))
                    || context.Usuarios.Any(u => u.IdUsuario == c.UsuarioId
                                                 && (u.Email.Contains(texto) || u.Apellido.Contains(texto) || u.Nombre.Contains(texto))));
            }

            // Sin orden elegido: la más nueva primero.
            bool desc = filtro.OrdenarPor is null || filtro.EsDescendente;
            var ordenada = filtro.OrdenarPor switch
            {
                "id" => consulta.OrdenarPor(c => c.CompraId, desc),
                "monto" => consulta.OrdenarPor(c => c.MontoTotal, desc),
                "cantidad" => consulta.OrdenarPor(c => c.Cantidad, desc),
                _ => consulta.OrdenarPor(c => c.FechaCompra, desc)
            };
            var pagina = await ordenada.LuegoPor(c => c.CompraId, desc).PaginarAsync(filtro, ct);

            // Nombres solo de lo que aparece en la página (consultas chicas con IN (...)).
            var usuarioIds = pagina.Items.Select(c => c.UsuarioId).Distinct().ToList();
            var eventoIds = pagina.Items.SelectMany(c => c.Entradas).Select(e => e.EventoId).Distinct().ToList();
            var sectorIds = pagina.Items.SelectMany(c => c.Entradas).Select(e => e.SectorId).Distinct().ToList();
            var usuarios = await context.Usuarios.AsNoTracking().Where(u => usuarioIds.Contains(u.IdUsuario)).ToDictionaryAsync(u => u.IdUsuario, ct);
            var eventos = await context.Eventos.AsNoTracking().Where(e => eventoIds.Contains(e.IdEvento)).ToDictionaryAsync(e => e.IdEvento, ct);
            var sectores = await context.Sectores.AsNoTracking().Where(s => sectorIds.Contains(s.SectorId)).ToDictionaryAsync(s => s.SectorId, ct);

            return pagina.Map(c =>
            {
                // Una compra es siempre de un solo evento y sector (así la arma CompraService).
                var primera = c.Entradas.OrderBy(e => e.EntradaId).FirstOrDefault();
                usuarios.TryGetValue(c.UsuarioId, out var usuario);
                Evento? evento = primera is null ? null : eventos.GetValueOrDefault(primera.EventoId);
                Sector? sector = primera is null ? null : sectores.GetValueOrDefault(primera.SectorId);

                return new CompraDto
                {
                    CompraId = c.CompraId,
                    UsuarioId = c.UsuarioId,
                    FechaCompra = c.FechaCompra,
                    Cantidad = c.Cantidad,
                    MontoTotal = c.MontoTotal,
                    MetodoPago = c.MetodoPago,
                    PaymentId = c.PaymentId,
                    EstadoPago = c.EstadoPago,
                    Entradas = c.Entradas.OrderBy(e => e.EntradaId).Select(e => new EntradaDto
                    {
                        EntradaId = e.EntradaId,
                        CompraId = e.CompraId,
                        EventoId = e.EventoId,
                        SectorId = e.SectorId,
                        FilaAsiento = e.FilaAsiento,
                        CodigoQr = e.CodigoQr
                    }).ToList(),
                    UsuarioNombre = usuario is null ? null : $"{usuario.Nombre} {usuario.Apellido}",
                    UsuarioEmail = usuario?.Email,
                    EventoId = evento?.IdEvento,
                    EventoNombre = evento?.Nombre,
                    EventoFecha = evento?.Fecha,
                    SectorNombre = sector?.Nombre
                };
            });
        }

        // "Mis Entradas". Paso 1: en la base se agrupan las entradas del usuario por partido (GROUP BY),
        // así cada partido es UNA fila aunque venga de varias compras. Paso 2: se filtran, ordenan y paginan
        // esos grupos. Paso 3: se trae el detalle de entradas solo de los partidos de la página.
        // Paginar grupos (y no compras) evita partidos repetidos o cortados entre páginas.
        public async Task<PagedResult<PartidoConEntradasDto>> ObtenerMisEntradasAsync(int usuarioId, MisEntradasFiltro filtro, CancellationToken ct = default)
        {
            var ahora = DateTime.Now;

            var porEvento = from e in context.Entradas
                            join c in context.Compras on e.CompraId equals c.CompraId
                            where c.UsuarioId == usuarioId
                            group c by e.EventoId into g
                            select new { EventoId = g.Key, Cantidad = g.Count(), UltimaCompra = g.Max(x => x.FechaCompra) };

            var consulta = from g in porEvento
                           join ev in context.Eventos on g.EventoId equals ev.IdEvento
                           join local in context.Clubs on ev.ClubLocalId equals local.ClubId
                           join visitante in context.Clubs on ev.ClubVisitanteId equals visitante.ClubId
                           join estadio in context.Estadios on ev.EstadioId equals estadio.EstadioId
                           select new
                           {
                               g.EventoId,
                               g.Cantidad,
                               g.UltimaCompra,
                               ev.Nombre,
                               ev.Fecha,
                               ev.EstaCancelado,
                               ev.Descripcion,
                               Local = local.Nombre,
                               Visitante = visitante.Nombre,
                               Estadio = estadio.Nombre,
                               estadio.Ciudad
                           };

            if (filtro.Busqueda is not null)
            {
                var texto = filtro.Busqueda;
                consulta = consulta.Where(x => x.Nombre.Contains(texto) || x.Local.Contains(texto)
                                               || x.Visitante.Contains(texto) || x.Estadio.Contains(texto));
            }
            if (filtro.Desde.HasValue)
            {
                var desde = filtro.Desde.Value.Date;
                consulta = consulta.Where(x => x.Fecha >= desde);
            }
            if (filtro.Hasta.HasValue)
            {
                var hastaExclusivo = filtro.Hasta.Value.Date.AddDays(1);
                consulta = consulta.Where(x => x.Fecha < hastaExclusivo);
            }
            consulta = filtro.Estado?.ToLowerInvariant() switch
            {
                MisEntradasFiltro.EstadoProximos => consulta.Where(x => !x.EstaCancelado && x.Fecha > ahora),
                MisEntradasFiltro.EstadoPasados => consulta.Where(x => !x.EstaCancelado && x.Fecha <= ahora),
                MisEntradasFiltro.EstadoCancelados => consulta.Where(x => x.EstaCancelado),
                _ => consulta
            };

            // Orden por defecto: primero los próximos (el más cercano arriba) y después los ya jugados
            // (el más reciente arriba). Al ordenar por compra se usa la compra MÁS RECIENTE del grupo.
            // Siempre se desempata por id del evento para que el orden sea estable entre páginas.
            var ordenada = filtro.OrdenarPor switch
            {
                MisEntradasFiltro.OrdenPartidoLejano => consulta.OrderByDescending(x => x.Fecha).ThenBy(x => x.EventoId),
                MisEntradasFiltro.OrdenCompraReciente => consulta.OrderByDescending(x => x.UltimaCompra).ThenBy(x => x.EventoId),
                MisEntradasFiltro.OrdenCompraAntigua => consulta.OrderBy(x => x.UltimaCompra).ThenBy(x => x.EventoId),
                _ => consulta.OrderBy(x => x.Fecha > ahora ? 0 : 1)
                             .ThenBy(x => x.Fecha > ahora ? (DateTime?)x.Fecha : null)
                             .ThenByDescending(x => x.Fecha)
                             .ThenBy(x => x.EventoId)
            };
            var pagina = await ordenada.PaginarAsync(filtro, ct);

            var ids = pagina.Items.Select(x => x.EventoId).ToList();
            var detalle = await (from e in context.Entradas.AsNoTracking()
                                 join c in context.Compras on e.CompraId equals c.CompraId
                                 join s in context.Sectores on e.SectorId equals s.SectorId
                                 where c.UsuarioId == usuarioId && ids.Contains(e.EventoId)
                                 orderby c.FechaCompra, e.EntradaId
                                 select new
                                 {
                                     e.EventoId,
                                     c.MontoTotal,
                                     Entrada = new MiEntradaDto
                                     {
                                         EntradaId = e.EntradaId,
                                         CompraId = c.CompraId,
                                         FechaCompra = c.FechaCompra,
                                         SectorNombre = s.Nombre,
                                         EstadoPago = c.EstadoPago,
                                         MetodoPago = c.MetodoPago,
                                         CodigoQr = e.CodigoQr
                                     }
                                 }).ToListAsync(ct);

            return pagina.Map(x =>
            {
                var propias = detalle.Where(d => d.EventoId == x.EventoId).ToList();
                // El total se suma por compra (no por entrada) para no contar el monto dos veces.
                var montosPorCompra = propias.GroupBy(d => d.Entrada.CompraId).Select(g => g.First().MontoTotal).ToList();
                return new PartidoConEntradasDto
                {
                    EventoId = x.EventoId,
                    EventoNombre = x.Nombre,
                    Fecha = x.Fecha,
                    ClubLocal = x.Local,
                    ClubVisitante = x.Visitante,
                    EstadioNombre = x.Estadio,
                    EstadioCiudad = x.Ciudad,
                    Descripcion = x.Descripcion,
                    EstaCancelado = x.EstaCancelado,
                    CantidadEntradas = x.Cantidad,
                    CantidadCompras = montosPorCompra.Count,
                    TotalPagado = montosPorCompra.Sum(),
                    UltimaCompra = x.UltimaCompra,
                    Entradas = propias.Select(d => d.Entrada).ToList()
                };
            });
        }
    }
}
