using Core.Interfaces;
using Data.Implementaciones;
using Data.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;

        public ClubService(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<Club> CrearClubAsync(Club club)
        {
            ValidarClub(club);

            await _clubRepository.AddAsync(club);
            return club;
        }

        public async Task<Club?> ObtenerPorIdAsync(int id)
        {
            return await _clubRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Club>> ObtenerTodosAsync()
        {
            return await _clubRepository.GetAllAsync();
        }

        public async Task<bool> ActualizarClubAsync(Club club)
        {
            ValidarClub(club);

            return await _clubRepository.UpdateAsync(club);
        }

        public async Task<bool> EliminarClubAsync(int id)
        {
            return await _clubRepository.DeleteAsync(id);
        }

        private static void ValidarClub(Club club)
        {
            if (club is null)
            {
                throw new ArgumentException("El club no puede ser nulo.");
            }
            if (string.IsNullOrWhiteSpace(club.Nombre))
            {
                throw new ArgumentException("El nombre del club es obligatorio.");
            }
            if (string.IsNullOrWhiteSpace(club.Abreviatura))
            {
                throw new ArgumentException("La abreviatura del club es obligatoria.");
            }
        }
    }
}