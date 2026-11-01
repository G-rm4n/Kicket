using Core.Interfaces;
using Domain.Entities;
using Kicket.Contracts.Entradas;

namespace WebApi.EndPoints
{
    /// <summary>
    /// Solo lectura: una Entrada siempre nace dentro de una Compra (POST /compras la crea).
    /// No hay POST/PUT/DELETE acá, ni un "traer todas" — IEntradaService solo permite
    /// consultar por id o por evento.
    /// </summary>
    public static class EntradaEndPoints
    {
        // Registra las rutas de consulta de entradas (requieren estar logueado).
        public static void MapEntradaEndPoints(this WebApplication app)
        {
            // GET /entradas/{id}: devuelve una entrada o 404.
            app.MapGet("/entradas/{id}", async (int id, IEntradaService entradaService) =>
            {
                Entrada? entrada = await entradaService.ObtenerPorIdAsync(id);

                if (entrada is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(MapearAEntradaDto(entrada));
            })
            .WithName("GetEntrada")
            .Produces<EntradaDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();

            // GET /entradas/evento/{id}: lista las entradas vendidas para un evento.
            app.MapGet("/entradas/evento/{eventoId}", async (int eventoId, IEntradaService entradaService) =>
            {
                var entradas = await entradaService.ObtenerPorEventoAsync(eventoId);

                IEnumerable<EntradaDto> dtos = entradas.Select(MapearAEntradaDto).ToList();

                return Results.Ok(dtos);
            })
            .WithName("GetEntradasPorEvento")
            .Produces<IEnumerable<EntradaDto>>(StatusCodes.Status200OK)
            .RequireAuthorization();
        }

        // Convierte una Entrada a EntradaDto.
        private static EntradaDto MapearAEntradaDto(Entrada entrada)
        {
            return new EntradaDto
            {
                EntradaId = entrada.EntradaId,
                CompraId = entrada.CompraId,
                EventoId = entrada.EventoId,
                SectorId = entrada.SectorId,
                FilaAsiento = entrada.FilaAsiento,
                CodigoQr = entrada.CodigoQr
            };
        }
    }
}
