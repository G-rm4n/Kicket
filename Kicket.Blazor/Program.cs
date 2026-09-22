using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Clientes;
using Kicket.ApiClient.Configuracion;
using Kicket.ApiClient.Http;
using Kicket.ApiClient.Sesion;
using Kicket.Blazor.Components;
using Kicket.Blazor.Core.session;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTransient<IClubApiClient, ClubApiClient>();
builder.Services.AddTransient<IUsuarioApiClient, UsuarioApiClient>();
builder.Services.AddTransient<IAuthApiClient, AuthApiClient>();
builder.Services.AddTransient<IEstadioApiClient, EstadioApiClient>();

builder.Services.AddKicketApiClient(options =>
{
    options.TimeoutSegundos = 30;
    options.BaseUrl = "http://localhost:5268/";
});

builder.Services.AddScoped<BlazorSesionUsuario>();
builder.Services.AddScoped<ISesionUsuario>(sp => sp.GetRequiredService<BlazorSesionUsuario>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
