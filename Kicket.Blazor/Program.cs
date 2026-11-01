// Punto de entrada del frontend Blazor Server: registra servicios, sesión, autenticación y la capa cliente de la API.
using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Configuracion;
using Kicket.Blazor.Components;
using Kicket.Blazor.Core.session;
using Kicket.Blazor.Core.state;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Kicket.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Sesión del usuario guardada en el storage protegido del navegador (una por conexión/circuito).
builder.Services.AddScoped<BlazorSesionUsuario>();
builder.Services.AddScoped<ISesionUsuario>(sp => sp.GetRequiredService<BlazorSesionUsuario>());

// Autenticación de Blazor basada en el proveedor propio KicketAuthStateProvider.
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<KicketAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<KicketAuthStateProvider>());

// Registra los clientes HTTP de la API apuntando a la URL donde corre WebApi.
builder.Services.AddKicketApiClient(options =>
{
    options.TimeoutSegundos = 30;
    options.BaseUrl = "http://localhost:5268/";
});

// Construye la aplicación.
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

// Registra el componente raíz App con render interactivo en servidor.
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Arranca el servidor.
app.Run();
