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
                c.Property(c => c.Ciudad).HasMaxLength(100);

                c.HasData(
                    new { ClubId = -1, Nombre = "Newell's Old Boys", Descripcion = "Club historico rosarino", Abreviatura = "NOB", Ciudad = "Rosario", AnioFundacion = 1903 },
                    new { ClubId = -2, Nombre = "Rosario Central", Descripcion = "Club atletico rosarino", Abreviatura = "CARC", Ciudad = "Rosario", AnioFundacion = 1889 },
                    new { ClubId = -3, Nombre = "Boca Juniors", Descripcion = "Club Atletico Boca Juniors", Abreviatura = "CABJ", Ciudad = "Buenos Aires", AnioFundacion = 1905 },
                    new { ClubId = -4, Nombre = "River Plate", Descripcion = "Club Atletico River Plate", Abreviatura = "CARP", Ciudad = "Buenos Aires", AnioFundacion = 1901 }
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
                    },
                    // Compras de ejemplo (todas aprobadas). Lucia (-2) tiene dos compras del mismo partido (-2)
                    // para mostrar el agrupamiento de "Mis Entradas"; la -3 y la -7 son de un partido ya jugado.
                    new { CompraId = -2, UsuarioId = -2, FechaCompra = new DateTime(2026, 2, 3, 10, 15, 0), Cantidad = 3, MontoTotal = 54000m, MetodoPago = "MercadoPago", PaymentId = "MP-SEED-20002", EstadoPago = "Aprobado" },
                    new { CompraId = -3, UsuarioId = -2, FechaCompra = new DateTime(2026, 3, 1, 18, 40, 0), Cantidad = 2, MontoTotal = 16000m, MetodoPago = "MercadoPago", PaymentId = "MP-SEED-20003", EstadoPago = "Aprobado" },
                    new { CompraId = -4, UsuarioId = -3, FechaCompra = new DateTime(2026, 2, 14, 12, 0, 0), Cantidad = 4, MontoTotal = 36000m, MetodoPago = "MercadoPago", PaymentId = "MP-SEED-20004", EstadoPago = "Aprobado" },
                    new { CompraId = -5, UsuarioId = -2, FechaCompra = new DateTime(2026, 5, 20, 9, 30, 0), Cantidad = 1, MontoTotal = 9000m, MetodoPago = "MercadoPago", PaymentId = "MP-SEED-20005", EstadoPago = "Aprobado" },
                    new { CompraId = -6, UsuarioId = -3, FechaCompra = new DateTime(2026, 6, 5, 20, 10, 0), Cantidad = 2, MontoTotal = 30000m, MetodoPago = "MercadoPago", PaymentId = "MP-SEED-20006", EstadoPago = "Aprobado" },
                    new { CompraId = -7, UsuarioId = -3, FechaCompra = new DateTime(2026, 3, 28, 16, 0, 0), Cantidad = 2, MontoTotal = 30000m, MetodoPago = "MercadoPago", PaymentId = "MP-SEED-20007", EstadoPago = "Aprobado" }
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
                    new { EntradaId = -2, EventoId = -1, SectorId = -1, CompraId = -1, FilaAsiento = "Fila 5, Asiento 11", CodigoQr = "KCKT-SEED00000000000000000000002" },
                    // Compra -2: Central vs Boca, Platea Norte (numerada).
                    new { EntradaId = -3, EventoId = -2, SectorId = -3, CompraId = -2, FilaAsiento = "Fila 1, Asiento 1", CodigoQr = "KCKT-SEED00000000000000000000003" },
                    new { EntradaId = -4, EventoId = -2, SectorId = -3, CompraId = -2, FilaAsiento = "Fila 1, Asiento 2", CodigoQr = "KCKT-SEED00000000000000000000004" },
                    new { EntradaId = -5, EventoId = -2, SectorId = -3, CompraId = -2, FilaAsiento = "Fila 1, Asiento 3", CodigoQr = "KCKT-SEED00000000000000000000005" },
                    // Compra -3: Newell's vs River (ya jugado), Popular Sur (sin numerar).
                    new { EntradaId = -6, EventoId = -3, SectorId = -2, CompraId = -3, CodigoQr = "KCKT-SEED00000000000000000000006" },
                    new { EntradaId = -7, EventoId = -3, SectorId = -2, CompraId = -3, CodigoQr = "KCKT-SEED00000000000000000000007" },
                    // Compra -4: Central vs Boca, Popular Norte.
                    new { EntradaId = -8, EventoId = -2, SectorId = -4, CompraId = -4, CodigoQr = "KCKT-SEED00000000000000000000008" },
                    new { EntradaId = -9, EventoId = -2, SectorId = -4, CompraId = -4, CodigoQr = "KCKT-SEED00000000000000000000009" },
                    new { EntradaId = -10, EventoId = -2, SectorId = -4, CompraId = -4, CodigoQr = "KCKT-SEED00000000000000000000010" },
                    new { EntradaId = -11, EventoId = -2, SectorId = -4, CompraId = -4, CodigoQr = "KCKT-SEED00000000000000000000011" },
                    // Compra -5: segunda compra de Lucia para Central vs Boca.
                    new { EntradaId = -12, EventoId = -2, SectorId = -4, CompraId = -5, CodigoQr = "KCKT-SEED00000000000000000000012" },
                    // Compra -6: Clasico Rosarino, Platea Este.
                    new { EntradaId = -13, EventoId = -1, SectorId = -1, CompraId = -6, FilaAsiento = "Fila 5, Asiento 12", CodigoQr = "KCKT-SEED00000000000000000000013" },
                    new { EntradaId = -14, EventoId = -1, SectorId = -1, CompraId = -6, FilaAsiento = "Fila 5, Asiento 13", CodigoQr = "KCKT-SEED00000000000000000000014" },
                    // Compra -7: Newell's vs River (ya jugado), Platea Este.
                    new { EntradaId = -15, EventoId = -3, SectorId = -1, CompraId = -7, FilaAsiento = "Fila 1, Asiento 1", CodigoQr = "KCKT-SEED00000000000000000000015" },
                    new { EntradaId = -16, EventoId = -3, SectorId = -1, CompraId = -7, FilaAsiento = "Fila 1, Asiento 2", CodigoQr = "KCKT-SEED00000000000000000000016" }
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
                e.Property(e => e.Provincia).HasMaxLength(100);

                e.HasData(
                    new { EstadioId = -1, Nombre = "Coloso del Parque", Direccion = "Parque Independencia", Ciudad = "Rosario", Provincia = "Santa Fe" },
                    new { EstadioId = -2, Nombre = "Gigante de Arroyito", Direccion = "Av. Genova y Bv. Avellaneda", Ciudad = "Rosario", Provincia = "Santa Fe" }
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
                e.Property(e => e.Descripcion).HasMaxLength(500);

                // Eventos de ejemplo: dos proximos, uno ya jugado (-3) y uno cancelado (-4).
                e.HasData(
                    new
                    {
                        IdEvento = -1,
                        Nombre = "Clásico Rosarino",
                        Fecha = new DateTime(2027, 12, 15, 17, 0, 0),
                        EstadioId = -1,
                        ClubLocalId = -1,
                        ClubVisitanteId = -2,
                        EstaCancelado = false,
                        Descripcion = "Apertura de puertas dos horas antes del inicio. Presentar DNI junto con la entrada."
                    },
                    new { IdEvento = -2, Nombre = "Rosario Central vs Boca Juniors", Fecha = new DateTime(2027, 3, 20, 21, 0, 0), EstadioId = -2, ClubLocalId = -2, ClubVisitanteId = -3, EstaCancelado = false, Descripcion = "Fecha 6 del torneo. Sin publico visitante." },
                    new { IdEvento = -3, Nombre = "Newell's vs River Plate", Fecha = new DateTime(2026, 4, 12, 19, 0, 0), EstadioId = -1, ClubLocalId = -1, ClubVisitanteId = -4, EstaCancelado = false },
                    new { IdEvento = -4, Nombre = "Rosario Central vs River Plate", Fecha = new DateTime(2027, 5, 9, 18, 0, 0), EstadioId = -2, ClubLocalId = -2, ClubVisitanteId = -4, EstaCancelado = true, Descripcion = "Partido suspendido por la organizacion." }
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
                    new { SectorId = -2, EstadioId = -1, Nombre = "Popular Sur", CapacidadMaxima = 12000, PrecioBase = 8000m },
                    new { SectorId = -3, EstadioId = -2, Nombre = "Platea Norte", CapacidadMaxima = 4000, PrecioBase = 18000m },
                    new { SectorId = -4, EstadioId = -2, Nombre = "Popular Norte", CapacidadMaxima = 10000, PrecioBase = 9000m }
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
                // DNI unico, pero solo entre los que lo tienen cargado (indice filtrado): los usuarios
                // viejos sin DNI pueden convivir con NULL sin chocar entre ellos.
                u.Property(u => u.Dni).HasMaxLength(10);
                u.HasIndex(u => u.Dni).IsUnique().HasFilter("[Dni] IS NOT NULL");
                u.Property(u => u.Telefono).HasMaxLength(30);

                u.HasData(
                    new
                    {
                        IdUsuario = -1,
                        Nombre = "Pepe",
                        Apellido = "Perez",
                        Email = "EmailRaro@gmail.com",
                        Password = "$2b$11$IyiQ5PdqWBqL9tDUJ8Na.eUwzGSa/ZGjLxM.gXc.anaEX8nT46bQG", // BCrypt de "Has"
                        FechaRegistro = new DateTime(2024, 1, 1),
                        Rol = "Admin",
                        Dni = "30111222",
                        Telefono = "341 555-0101"
                    },
                    // Clientes de ejemplo (misma contraseña de prueba que el admin: "Has").
                    new { IdUsuario = -2, Nombre = "Lucia", Apellido = "Fernandez", Email = "lucia.fernandez@mail.com", Password = "$2b$11$IyiQ5PdqWBqL9tDUJ8Na.eUwzGSa/ZGjLxM.gXc.anaEX8nT46bQG", FechaRegistro = new DateTime(2025, 2, 10), Rol = "Cliente", Dni = "38456789", Telefono = "341 555-0123", FechaNacimiento = new DateTime(1995, 6, 14) },
                    new { IdUsuario = -3, Nombre = "Martin", Apellido = "Gomez", Email = "martin.gomez@mail.com", Password = "$2b$11$IyiQ5PdqWBqL9tDUJ8Na.eUwzGSa/ZGjLxM.gXc.anaEX8nT46bQG", FechaRegistro = new DateTime(2025, 8, 22), Rol = "Cliente", Dni = "40123987", FechaNacimiento = new DateTime(1998, 9, 3) }
                );
            });

        }
    }
}
