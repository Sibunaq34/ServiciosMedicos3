using MicroServicioUsuario;
using MicroServicioUsuario.Repository;
using MicroServicioUsuario.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddSingleton<SeguridadRepository>();
builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapAutenticacionEndpoints();

app.Run();
