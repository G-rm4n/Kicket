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
    public class ClubRepository : IClubRepository
    {
        private readonly TPIContext context;

        public ClubRepository(TPIContext context)
        {
            this.context = context;
        }

        public async Task<Club?> GetByIdAsync(int id)
        {
            return await context.Clubs.FirstOrDefaultAsync(c => c.ClubId == id);
        }

        public async Task<IEnumerable<Club>> GetAllAsync()
        {
            return await context.Clubs.ToListAsync();
        }

        public async Task AddAsync(Club club)
        {
            context.Clubs.Add(club);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var clubFound = await context.Clubs.FirstOrDefaultAsync(c => c.ClubId == id);
            if (clubFound is not null)
            {
                context.Clubs.Remove(clubFound);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

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