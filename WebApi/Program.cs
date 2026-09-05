

using System.Text;
using Core.Interfaces;
using Core.Services;
using Data;
using Data.Implementaciones;
using Data.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.EndPoints;

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
});

builder.Services.AddAuthorization();



//Mapeo de Repository

builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<IEstadioRepository, EstadioRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

//Mapeo de Services

builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IEstadioService, EstadioService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

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

var conectionString = builder.Configuration.GetConnectionString("Local");

//Mapeo de los EndPoints base, comentado hasta que se 
//Implementen los services.
app.MapClubEndPoints();
//app.MapCompraEndPoints();
//app.MapEntradaEndPoints();
//app.MapEventoEndPoints();
app.MapUsuarioEndPoints();
//app.MapSectorEndPoints();
app.MapEstadioEndPoints();
app.MapAuthEndPoints();

app.Run();

