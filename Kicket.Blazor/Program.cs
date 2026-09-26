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

builder.Services.AddScoped<BlazorSesionUsuario>();
builder.Services.AddScoped<ISesionUsuario>(sp => sp.GetRequiredService<BlazorSesionUsuario>());

builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<KicketAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<KicketAuthStateProvider>());

builder.Services.AddKicketApiClient(options =>
{
    options.TimeoutSegundos = 30;
    options.BaseUrl = "http://localhost:5268/";
});

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
