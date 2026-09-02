using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Domain.Entities;

namespace Data
{
    public class TPIContext:DbContext
    {
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Entrada> Entradas { get; set;}
        //public DbSet<Club> Clubs {get;set;}
        //public DbSet<Estadio> Estadios {get;set;}
        //public DbSet<Evento> Eventos {get;set;}
        //public DbSet<Sector> Sectores {get;set;}
        //public DbSet<Usuario> Usuarios {get;set;}

        public TPIContext(DbContextOptions<TPIContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
            modelBuilder.Entity<Club>((c) =>
            {
                //c.HasKey(c=>c.IdClub)
                //c.Property(c=>c.IdClub).ValueGeneratedOnAdd();
                //c.Property(c=>c.Nombre).HasMaxLength(100);
            });
            */

            modelBuilder.Entity<Compra>((c) =>
            {
                c.HasKey(c => c.CompraId);
                c.Property(c => c.CompraId).ValueGeneratedOnAdd();
                c.Property(c => c.Cantidad);
                c.HasMany<Entrada>(c =>  c.Entradas);

                c.HasData(new { CompraId=1, Cantidad=2,EventoId=2, FechaCompra=DateTime.Parse("2006/03/29"),MontoTotal=Decimal.Parse("10"),SectorId=1,UsuarioId=1 });

            });

            modelBuilder.Entity<Entrada>(e =>
            {
                e.HasKey(e => e.EntradaId);
                e.Property(e => e.EntradaId).ValueGeneratedOnAdd();
                e.Property(e => e.EventoId);
                e.Property(e => e.SectorId);
            });

        }
    }
}
