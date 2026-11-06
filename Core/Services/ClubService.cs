using Core.Interfaces;
using Data.Implementaciones;
using Data.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Clubes;
using Kicket.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Services
{
    // Lógica de negocio de clubes: valida los datos y delega en el repositorio.
    public class ClubService : IClubService
    {
        private readonly IClubRepository _clubRepository;

        // Recibe el repositorio de clubes por inyección de dependencias.
        public ClubService(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        // Valida el club y lo guarda.
        public async Task<Club> CrearClubAsync(Club club)
        {
            ValidarClub(club);

            await _clubRepository.AddAsync(club);
            return club;
        }

        // Devuelve un club por id.
        public async Task<Club?> ObtenerPorIdAsync(int id)
        {
            return await _clubRepository.GetByIdAsync(id);
        }

        // Devuelve todos los clubes.
        public async Task<IEnumerable<Club>> ObtenerTodosAsync()
        {
            return await _clubRepository.GetAllAsync();
        }

        // Valida los filtros y pide la pagina al repositorio.
        public async Task<PagedResult<ClubDto>> ObtenerPaginadoAsync(ClubFiltro filtro, CancellationToken ct = default)
        {
            Validaciones.ValidarFiltro(filtro);
            return await _clubRepository.ObtenerPaginadoAsync(filtro, ct);
        }

        // Valida el club y lo actualiza.
        public async Task<bool> ActualizarClubAsync(Club club)
        {
            ValidarClub(club);

            return await _clubRepository.UpdateAsync(club);
        }

        // Elimina el club. Si tiene eventos asociados, la BD lo impide (FK Restrict).
        public async Task<bool> EliminarClubAsync(int id)
        {
            return await _clubRepository.DeleteAsync(id);
        }

        // Reglas: el club no puede ser nulo y nombre y abreviatura son obligatorios (si no, ArgumentException → 400).
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
            club.Ciudad = Validaciones.TextoOpcional(club.Ciudad);
            if (club.Ciudad is { Length: > 100 })
            {
                throw new ArgumentException("La ciudad no puede superar los 100 caracteres.");
            }
            if (club.AnioFundacion.HasValue && (club.AnioFundacion < 1850 || club.AnioFundacion > DateTime.Today.Year))
            {
                throw new ArgumentException("El año de fundacion debe estar entre 1850 y el año actual.");
            }
        }
    }
}