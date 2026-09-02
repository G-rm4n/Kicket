

using Data;
using Microsoft.EntityFrameworkCore;
using WebApi.EndPoints;
using Core.Interfaces;
using Core.Services;

var builder = WebApplication.CreateBuilder(args);

//Se agrega el context de la DB del TP
builder.Services.AddDbContext<TPIContext>((options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});
builder.Services.AddScoped<ICompraService, CompraService>();

var app = builder.Build();
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

