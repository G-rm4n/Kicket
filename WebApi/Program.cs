// Punto de entrada de la API: configura servicios (BD, JWT, autorización, DI, Swagger),
// arma el pipeline HTTP y registra todos los endpoints.
using System.Text;
using Core.Interfaces;
using Core.Services;
using Data;
using Data.Implementaciones;
using Data.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.EndPoints;
using WebApi.Middlewares;

// Crea el builder de la aplicación web (lee appsettings, variables de entorno, etc.).
var builder = WebApplication.CreateBuilder(args);

//Se agrega el context de la DB del TP
builder.Services.AddDbContext<TPIContext>((options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

//Autenticacion
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
// Configura cómo se valida el token JWT: emisor, audiencia, vencimiento y firma.
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };

    // Eventos de depuración: muestran en consola si el token falló o se validó.
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"[JWT ERROR] Falló la autenticación: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("EL TOKEN SI SE VALIDO");
            return Task.CompletedTask;
        }
    };
});

// Autorización: la política "Solo admin" exige que el token tenga el rol Admin.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Solo admin", policy => policy.RequireRole("Admin"));
});

//Mapeo de Repository
builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<IEstadioRepository, EstadioRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ISectorRepository, SectorRepository>();
builder.Services.AddScoped<IEntradaRepository, EntradaRepository>();

//Mapeo de Services
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IEstadioService, EstadioService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<ISectorService, SectorService>();
builder.Services.AddScoped<IEntradaService, EntradaService>();
builder.Services.AddScoped<ICompraService, CompraService>();
builder.Services.AddScoped<IMercadoPagoService, MercadoPagoService>();

// Swagger: documentación y prueba de los endpoints en /swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra el manejador global de excepciones.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Construye la aplicación con todo lo registrado.
var app = builder.Build();

// Pipeline HTTP: primero el manejo de errores, después autenticación y autorización.
app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

// Habilita Swagger y su interfaz web.
app.UseSwagger();
app.UseSwaggerUI();

// Al arrancar, crea la base de datos si no existe (EnsureCreated, con los datos de seed).
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<TPIContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al crear la base de datos.");
    }
}

// Registra todos los endpoints de la API.
app.MapClubEndPoints();
app.MapCompraEndPoints();
app.MapEntradaEndPoints();
app.MapEventoEndPoints();
app.MapUsuarioEndPoints();
app.MapSectorEndPoints();
app.MapEstadioEndPoints();
app.MapAuthEndPoints();
app.MapPagoEndPoints();

// Arranca el servidor web.
app.Run();
