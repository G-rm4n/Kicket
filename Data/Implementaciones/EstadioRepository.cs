using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Consultas;
using Data.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Common;
using Kicket.Contracts.Estadios;
using Microsoft.EntityFrameworkCore;

namespace Data.Implementaciones
{
    // Implementación del repositorio de estadios usando EF Core.
    public class EstadioRepository:IEstadioRepository
    {
        private readonly TPIContext context;

        // Recibe el DbContext por inyección de dependencias.
        public EstadioRepository(TPIContext context)
        {
            this.context = context;
        }

        // Busca un estadio por id.
        public async Task<Estadio?> GetByIdAsync(int id)
        {
            return await context.Estadios.FirstOrDefaultAsync(e => e.EstadioId == id);
        }

        // Trae todos los estadios.
        public async Task<IEnumerable<Estadio>> GetAllAsync()
        {
            return await context.Estadios.ToListAsync();
        }

        // Pagina de estadios. La capacidad total y la cantidad de sectores se calculan en la misma
        // consulta (subconsultas sobre Sectores), no se guardan en la tabla para no duplicar datos.
        public async Task<PagedResult<EstadioDto>> ObtenerPaginadoAsync(EstadioFiltro filtro, CancellationToken ct = default)
        {
            IQueryable<Estadio> consulta = context.Estadios.AsNoTracking();

            if (filtro.Busqueda is not null)
            {
                var texto = filtro.Busqueda;
                consulta = consulta.Where(e => e.Nombre.Contains(texto) || e.Direccion.Contains(texto));
            }
            if (!string.IsNullOrWhiteSpace(filtro.Ciudad))
            {
                var ciudad = filtro.Ciudad.Trim();
                consulta = consulta.Where(e => e.Ciudad.Contains(ciudad));
            }
            if (!string.IsNullOrWhiteSpace(filtro.Provincia))
            {
                var provincia = filtro.Provincia.Trim();
                consulta = consulta.Where(e => e.Provincia != null && e.Provincia.Contains(provincia));
            }

            bool desc = filtro.EsDescendente;
            var ordenada = filtro.OrdenarPor switch
            {
                "id" => consulta.OrdenarPor(e => e.EstadioId, desc),
                "ciudad" => consulta.OrdenarPor(e => e.Ciudad, desc),
                "provincia" => consulta.OrdenarPor(e => e.Provincia, desc),
                _ => consulta.OrdenarPor(e => e.Nombre, desc)
            };

            return await ordenada.LuegoPor(e => e.EstadioId, desc)
                .Select(e => new EstadioDto
                {
                    IdEstadio = e.EstadioId,
                    Nombre = e.Nombre,
                    Direccion = e.Direccion,
                    Ciudad = e.Ciudad,
                    Provincia = e.Provincia,
                    CapacidadTotal = context.Sectores.Where(s => s.EstadioId == e.EstadioId).Sum(s => (int?)s.CapacidadMaxima) ?? 0,
                    CantidadSectores = context.Sectores.Count(s => s.EstadioId == e.EstadioId)
                })
                .PaginarAsync(filtro, ct);
        }

        // Agrega un estadio y guarda.
        public async Task AddAsync(Estadio estadio)
        {
            context.Estadios.Add(estadio);
            await context.SaveChangesAsync();
        }

        // Busca el estadio; si existe lo borra y devuelve true; si no, false.
        public async Task<bool> DeleteAsync(int id)
        {
            var estadioFound = await context.Estadios.FirstOrDefaultAsync(e => e.EstadioId == id);
            if(estadioFound is not null)
            {
                context.Estadios.Remove(estadioFound);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Busca el estadio existente, copia dirección, ciudad, provincia y nombre, y guarda. False si no existe.
        public async Task<bool> UpdateAsync(Estadio estadio)
        {
            var estadioFound = await context.Estadios.FirstOrDefaultAsync(e => e.EstadioId == estadio.EstadioId);
            if(estadioFound is not null)
            {
                estadioFound.Direccion = estadio.Direccion;
                estadioFound.Ciudad = estadio.Ciudad;
                estadioFound.Provincia = estadio.Provincia;
                estadioFound.Nombre = estadio.Nombre;

                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
