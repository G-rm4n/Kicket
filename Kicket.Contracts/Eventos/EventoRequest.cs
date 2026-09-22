using System;
using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Eventos
{
    /// <summary>Datos para dar de alta un evento (POST /eventos).</summary>
    public class EventoRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 150 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El estadio es obligatorio.")]
        public int EstadioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El club local es obligatorio.")]
        public int ClubLocalId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El club visitante es obligatorio.")]
        public int ClubVisitanteId { get; set; }
    }
}