using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kicket.Contracts.Compras
{

    /// <summary>
    /// Datos para generar una compra (POST /compras). No tiene UpdateRequest: una compra
    /// no se edita, en todo caso se cancela/elimina (ver ICompraService).
    /// </summary>
    public class CompraRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "El usuario es obligatorio.")]
        public int UsuarioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El evento es obligatorio.")]
        public int EventoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El sector es obligatorio.")]
        public int SectorId { get; set; }

        [Range(1, 20, ErrorMessage = "La cantidad de entradas debe ser entre 1 y 20.")]
        public int Cantidad { get; set; }
    }
}
