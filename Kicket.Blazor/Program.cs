using Kicket.ApiClient.Abstracciones;
using Kicket.ApiClient.Clientes;
using Kicket.ApiClient.Sesion;
using Kicket.Blazor.Components;
using Kicket.Blazor.Core.Interceptors;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ISesionUsuario, SesionUsuario>();
builder.Services.AddTransient<TokenInsertor>();

builder.Services.AddHttpClient("UsuarioApiClient").AddHttpMessageHandler<TokenInsertor>();

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
