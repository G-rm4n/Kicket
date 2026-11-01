using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Domain.Entities;
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

        // Busca el club existente, copia los campos editables (nombre, descripción, abreviatura) y guarda.
        // Devuelve false si el club no existe.
        public async Task<bool> UpdateAsync(Club club)
        {
            var clubFound = await context.Clubs.FirstOrDefaultAsync(c => c.ClubId == club.ClubId);
            if (clubFound is not null)
            {
                clubFound.Nombre = club.Nombre;
                clubFound.Descripcion = club.Descripcion;
                clubFound.Abreviatura = club.Abreviatura;

                await context.SaveChangesAsync();
                return true;

                
            }
            return false;
        }
    }
}
