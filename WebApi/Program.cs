

using Data;
using Microsoft.EntityFrameworkCore;
using WebApi.EndPoints;

var builder = WebApplication.CreateBuilder(args);

//Se agrega el context de la DB del TP
builder.Services.AddDbContext<TPIContext>((options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

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

