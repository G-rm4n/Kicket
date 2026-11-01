using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Domain.Entities;

namespace Data
{
    // DbContext de Entity Framework Core: mapea las entidades del dominio a tablas de SQL Server.
    public class TPIContext : DbContext
    {
        // Cada DbSet representa una tabla de la base de datos.
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Entrada> Entradas { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Estadio> Estadios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Sector> Sectores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // Constructor: recibe las opciones (cadena de conexión) por inyección de dependencias
        // y crea la base de datos si no existe (EnsureCreated, sin migraciones).
        public TPIContext(DbContextOptions<TPIContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        // Configura el modelo: claves, relaciones, tamaños, valores por defecto y datos iniciales (seed).
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // --- Club: PK autoincremental, largos máximos y 2 clubes de ejemplo (ids negativos).
            modelBuilder.Entity<Club>((c) =>
            {
                c.HasKey(c => c.ClubId);
                c.Property(c => c.ClubId).ValueGeneratedOnAdd();
                c.Property(c => c.Nombre).HasMaxLength(100);
                c.Property(c => c.Descripcion).HasMaxLength(255);
                c.Property(c => c.Abreviatura).HasMaxLength(10);

                c.HasData(
                    new { ClubId = -1, Nombre = "Newell's Old Boys", Descripcion = "Club historico rosarino", Abreviatura = "NOB" },
                    new { ClubId = -2, Nombre = "Rosario Central", Descripcion = "Club atletico rosarino", Abreviatura = "CARC" }
                );
            });


            // --- Compra: PK, FK a Usuario (borrado en cascada por defecto), decimales, defaults y 1 compra de ejemplo.
            modelBuilder.Entity<Compra>((c) =>
            {
                c.HasKey(c => c.CompraId);
                c.Property(c => c.CompraId).ValueGeneratedOnAdd();
                c.HasOne<Usuario>().WithMany().HasForeignKey(c => c.UsuarioId);
                c.Property(c => c.FechaCompra);
                c.Property(c => c.Cantidad);
                c.Property(c => c.MontoTotal).HasColumnType("decimal(18,2)");
                c.Property(c => c.MetodoPago).HasMaxLength(50).HasDefaultValue("MercadoPago");
                c.Property(c => c.PaymentId).HasMaxLength(100);
                c.Property(c => c.EstadoPago).HasMaxLength(50).HasDefaultValue("Aprobado");
                c.HasMany<Entrada>(c => c.Entradas).WithOne();

                c.HasData(
                    new
                    {
                        CompraId = -1,
                        UsuarioId = -1,
                        FechaCompra = new DateTime(2024, 11, 20),
                        Cantidad = 2,
                        MontoTotal = 30000m, // (2 entradas de Platea a 15000)
                        MetodoPago = "MercadoPago",
                        PaymentId = "MP-SEED-12345",
                        EstadoPago = "Aprobado"
                    }
                );

            });

            // --- Entrada: PK, FK a Evento y Sector con Restrict (no se pueden borrar si tienen entradas),
            // FK a Compra (cascada), QR obligatorio y 2 entradas de ejemplo.
            modelBuilder.Entity<Entrada>(e =>
            {
                e.HasKey(e => e.EntradaId);
                e.Property(e => e.EntradaId).ValueGeneratedOnAdd();
                e.HasOne<Evento>().WithMany().HasForeignKey(e => e.EventoId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne<Sector>().WithMany().HasForeignKey(e => e.SectorId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne<Compra>().WithMany(c => c.Entradas).HasForeignKey(e => e.CompraId);
                e.Property(e => e.FilaAsiento);
                e.Property(e => e.CodigoQr).HasMaxLength(128).IsRequired();

                e.HasData(
                    new { EntradaId = -1, EventoId = -1, SectorId = -1, CompraId = -1, FilaAsiento = "Fila 5, Asiento 10", CodigoQr = "KCKT-SEED00000000000000000000001" },
                    new { EntradaId = -2, EventoId = -1, SectorId = -1, CompraId = -1, FilaAsiento = "Fila 5, Asiento 11", CodigoQr = "KCKT-SEED00000000000000000000002" }
                );
            });

            // --- Estadio: PK y 1 estadio de ejemplo.
            modelBuilder.Entity<Estadio>(e =>
            {
                e.HasKey(e => e.EstadioId);
                e.Property(e => e.EstadioId).ValueGeneratedOnAdd();
                e.Property(e => e.Ciudad);
                e.Property(e => e.Direccion);
                e.Property(e => e.Nombre);

                e.HasData(
                    new { EstadioId = -1, Nombre = "Coloso del Parque", Direccion = "Parque Independencia", Ciudad = "Rosario" }
                );
            });

            // --- Evento: PK, FK a Estadio (cascada), FK a club local y visitante (Restrict),
            // EstaCancelado = false por defecto y 1 evento de ejemplo.
            modelBuilder.Entity<Evento>(e =>
            {
                e.HasKey(e => e.IdEvento);
                e.Property(e => e.IdEvento).ValueGeneratedOnAdd();
                e.Property(e => e.Fecha);
                e.HasOne<Estadio>().WithMany().HasForeignKey(e => e.EstadioId);
                e.HasOne<Club>().WithMany().HasForeignKey(c => c.ClubVisitanteId).OnDelete(DeleteBehavior.Restrict); ;
                e.HasOne<Club>().WithMany().HasForeignKey(c => c.ClubLocalId).OnDelete(DeleteBehavior.Restrict); ;
                e.Property(e => e.EstaCancelado).HasDefaultValue(false);
                e.Property(e => e.Nombre);

                e.HasData(
                    new
                    {
                        IdEvento = -1,
                        Nombre = "Clásico Rosarino",
                        Fecha = new DateTime(2027, 12, 15, 17, 0, 0),
                        EstadioId = -1,
                        ClubLocalId = -1,
                        ClubVisitanteId = -2,
                        EstaCancelado = false
                    }
                );
            });

            // --- Sector: PK, precio decimal, FK a Estadio (Restrict) y 2 sectores de ejemplo.
            modelBuilder.Entity<Sector>(s =>
            {
                s.HasKey(s => s.SectorId);
                s.Property(s => s.SectorId).ValueGeneratedOnAdd();
                s.Property(s => s.EstadioId);
                s.Property(s => s.Nombre);
                s.Property(s => s.CapacidadMaxima);
                s.Property(s => s.PrecioBase).HasColumnType("decimal(18,2)");
                s.HasOne<Estadio>().WithMany().HasForeignKey(s => s.EstadioId).OnDelete(DeleteBehavior.Restrict);

                s.HasData(
                    new { SectorId = -1, EstadioId = -1, Nombre = "Platea Este", CapacidadMaxima = 5000, PrecioBase = 15000m },
                    new { SectorId = -2, EstadioId = -1, Nombre = "Popular Sur", CapacidadMaxima = 12000, PrecioBase = 8000m }
                );
            });

            // --- Usuario: PK, email único (índice), hash de 100 caracteres, rol por defecto
            // "Usuario" y un administrador inicial (EmailRaro@gmail.com / Has).
            modelBuilder.Entity<Usuario>(u =>
            {
                u.HasKey(u => u.IdUsuario);
                u.Property(u => u.IdUsuario).ValueGeneratedOnAdd();
                u.Property(u => u.Nombre);
                u.Property(u => u.Apellido);
                u.HasIndex(u => u.Email).IsUnique();
                u.Property(u => u.Password).HasMaxLength(100); // hash BCrypt = 60 caracteres
                u.Property(u => u.FechaRegistro);
                u.Property(u => u.Rol).HasDefaultValue("Usuario");

                u.HasData(
                    new
                    {
                        IdUsuario = -1,
                        Nombre = "Pepe",
                        Apellido = "Perez",
                        Email = "EmailRaro@gmail.com",
                        Password = "$2b$11$IyiQ5PdqWBqL9tDUJ8Na.eUwzGSa/ZGjLxM.gXc.anaEX8nT46bQG", // BCrypt de "Has"
                        FechaRegistro = new DateTime(2024, 1, 1),
                        Rol = "Admin"
                    }
                );
            });

        }
    }
}
