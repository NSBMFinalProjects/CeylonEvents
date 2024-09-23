using frontend.Authentication;
using frontend.Clients;
using frontend.Components;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddOutputCache();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

var eventHandlerAPI = builder.Configuration["EventHandlerApi"] ?? throw new Exception("Backend URL not found");
builder.Services.AddHttpClient<UserClient>(client => client.BaseAddress = new Uri(eventHandlerAPI));


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

app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
