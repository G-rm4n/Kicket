using Kiketpropio.Services.Interfaces;
using Kiketpropio.Services.Implementaciones;

var builder = WebApplication.CreateBuilder(args);

//Inyeccion de dependencias
builder.Services.AddSingleton<IUsuarioService, UsuarioService>();
builder.Services.AddSingleton<IEstadioService, EstadioService>();
builder.Services.AddSingleton<IClubService, ClubService>();

//Capa de controladores
builder.Services.AddControllers();

//Agregar servicios definidos
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
