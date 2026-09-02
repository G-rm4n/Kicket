using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Interfaces;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Implementaciones
{
    public class CompraRepository : ICompraRepository
    {
        private readonly TPIContext context;

        public CompraRepository(TPIContext context)
        {
            this.context=context;
        }

        public async Task<IEnumerable<Compra>> GetAllAsync()
        {
            return await context.Compras.ToListAsync();
        }

        public async Task<Compra?> GetByIdAsync(int id)
        {
            return await context.Compras.FirstOrDefaultAsync(c => c.CompraId == id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var compraFound = await context.Compras.FirstOrDefaultAsync(e => e.CompraId == id);
            if (compraFound is not null)
            {
                context.Compras.Remove(compraFound);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task AddAsync(Compra compra)
        {
            context.Compras.Add(compra);
            await context.SaveChangesAsync();
        }

        public async Task<int> ObtenerCantidadEntradasVendidasAsync(int eventoId, int sectorId)
        {
            string query = @"
                SELECT count(*)
                FROM Compras comp
                INNER JOIN Entradas ent ON ent.CompraId=comp.CompraId
                WHERE ent.EventoId=@eventoId and ent.SectorId=@sectorId";

            string conectionString = context.Database.GetConnectionString()!;
            using var connection = new SqlConnection(conectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@eventoId", eventoId);
            command.Parameters.AddWithValue("@sectorId", sectorId);

            await connection.OpenAsync();

            var resultado = await command.ExecuteScalarAsync();

            int cantidadEntradas = resultado != null ? Convert.ToInt32(resultado) : 0;
            return cantidadEntradas;
        }
    }
}
