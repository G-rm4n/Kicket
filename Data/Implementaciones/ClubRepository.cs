using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Consultas;
using Data.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Clubes;
using Kicket.Contracts.Common;
using Microsoft.EntityFrameworkCore;

namespace Data.Implementaciones
{
    // Implementación del repositorio de clubes usando EF Core (TPIContext).
    public class ClubRepository:IClubRepository
    {
        private readonly TPIContext context;

        // Recibe el DbContext por inyección de dependencias.
        public ClubRepository(TPIContext context)
        {
            this.context = context;
        }

        // Agrega el club al contexto y guarda los cambios en la BD.
        public async Task AddAsync(Club club)
        {
            context.Clubs.Add(club);
            await context.SaveChangesAsync();
        }

        // Busca el club por su id; devuelve null si no lo encuentra.
        public async Task<Club?> GetByIdAsync(int id)
        {
            return await context.Clubs.FirstOrDefaultAsync(c => c.ClubId == id);
        }

        // Trae todos los clubes de la tabla.
        public async Task<IEnumerable<Club>> GetAllAsync()
        {
            return await context.Clubs.ToListAsync();
        }

        // Pagina de clubes: arma la consulta con los filtros, la ordena y la resuelve con Count + Skip/Take.
        // Todo se traduce a SQL parametrizado (Contains => LIKE), nunca se concatena texto del usuario.
        public async Task<PagedResult<ClubDto>> ObtenerPaginadoAsync(ClubFiltro filtro, CancellationToken ct = default)
        {
            IQueryable<Club> consulta = context.Clubs.AsNoTracking();

            if (filtro.Busqueda is not null)
            {
                var texto = filtro.Busqueda;
                consulta = consulta.Where(c => c.Nombre.Contains(texto)
                                               || c.Abreviatura.Contains(texto)
                                               || (c.Descripcion != null && c.Descripcion.Contains(texto)));
            }
            if (!string.IsNullOrWhiteSpace(filtro.Ciudad))
            {
                var ciudad = filtro.Ciudad.Trim();
                consulta = consulta.Where(c => c.Ciudad != null && c.Ciudad.Contains(ciudad));
            }

            // Orden elegido + el id como desempate, para que el orden sea estable entre paginas.
            bool desc = filtro.EsDescendente;
            var ordenada = filtro.OrdenarPor switch
            {
                "id" => consulta.OrdenarPor(c => c.ClubId, desc),
                "abreviatura" => consulta.OrdenarPor(c => c.Abreviatura, desc),
                "ciudad" => consulta.OrdenarPor(c => c.Ciudad, desc),
                "fundacion" => consulta.OrdenarPor(c => c.AnioFundacion, desc),
                _ => consulta.OrdenarPor(c => c.Nombre, desc)
            };

            return await ordenada.LuegoPor(c => c.ClubId, desc)
                .Select(c => new ClubDto
                {
                    ClubId = c.ClubId,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Abreviatura = c.Abreviatura,
                    Ciudad = c.Ciudad,
                    AnioFundacion = c.AnioFundacion
                })
                .PaginarAsync(filtro, ct);
        }

        // Busca el club; si existe lo borra, guarda y devuelve true; si no, devuelve false.
        public async Task<bool> DeleteAsync(int id)
        {
            var clubFound = await context.Clubs.FirstOrDefaultAsync(c => c.ClubId == id);
            if(clubFound is not null)
            {
                context.Clubs.Remove(clubFound);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // Busca el club existente, copia los campos editables y guarda.
        // Devuelve false si el club no existe.
        public async Task<bool> UpdateAsync(Club club)
        {
            var clubFound = await context.Clubs.FirstOrDefaultAsync(c => c.ClubId == club.ClubId);
            if (clubFound is not null)
            {
                clubFound.Nombre = club.Nombre;
                clubFound.Descripcion = club.Descripcion;
                clubFound.Abreviatura = club.Abreviatura;
                clubFound.Ciudad = club.Ciudad;
                clubFound.AnioFundacion = club.AnioFundacion;

                await context.SaveChangesAsync();
                return true;


            }
            return false;
        }
    }
}
