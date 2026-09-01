using System.ComponentModel.DataAnnotations;

namespace Kicket.Contracts.Clubes
{
    /// <summary>
    /// Datos para modificar un club. El id viaja en el cuerpo y no en la ruta porque
    /// el endpoint definido por el equipo es PUT /clubes, sin parametro.
    /// </summary>
    public class ClubUpdateRequest : ClubRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "El id del club es obligatorio.")]
        public int IdClub { get; set; }
    }
}
