using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GestionOrdinateurs.Client;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Session
builder.Services.AddBlazoredSessionStorage();

// Autorisation
builder.Services.AddAuthorizationCore();

// Gestion de l'utilisateur connecté
builder.Services.AddScoped<JwtAuthenticationStateProvider>();

builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());

// Handler pour ajouter le JWT dans les requêtes
builder.Services.AddScoped<JwtHandler>();

// HttpClient
builder.Services.AddHttpClient("HttpClientName", client =>{client.BaseAddress = new Uri("http://localhost:5010/api/");}).AddHttpMessageHandler<JwtHandler>();

await builder.Build().RunAsync();