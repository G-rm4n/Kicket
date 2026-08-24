using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class TPIContext:DbContext
    {
        //public DbSet<Compra> Compras{get;set;};
        //public DbSet<Entrada> Entradas { get; set;};
        //public DbSet<Club> Clubs {get;set;};
        //public DbSet<Estadio> Estadios {get;set;};
        //public DbSet<Evento> Eventos {get;set;};
        //public DbSet<Sector> Sectores {get;set;};
        //public DbSet<Usuario> Usuarios {get;set;};

        public TPIContext(DbContextOptions<TPIContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }
    }
}
