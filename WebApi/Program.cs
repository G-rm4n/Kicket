

using Core.Interfaces;
using Core.Services;
using Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.EndPoints;

var builder = WebApplication.CreateBuilder(args);

//Se agrega el context de la DB del TP
builder.Services.AddDbContext<TPIContext>((options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});
//builder.Services.AddScoped<ICompraService, CompraService>();

var app = builder.Build();

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

app.MapGet("/", () => conectionString);

//Mapeo de los EndPoints base, comentado hasta que se 
//Implementen los services.
app.MapClubEndPoints();
app.MapCompraEndPoints();
app.MapEntradaEndPoints();
app.MapEventoEndPoints();
app.MapUsuarioEndPoints();
app.MapSectorEndPoints();
app.MapEstadioEndPoints();

app.Run();

