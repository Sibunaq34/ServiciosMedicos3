using MicroServicioUsuario;
using MicroServicioUsuario.Repository;
using MicroServicioUsuario.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<SeguridadRepository>();
builder.Services.AddScoped<IPasswordEncryptionService, PasswordEncryptionService>();
builder.Services.AddScoped<IAutenticacionService, AutenticacionService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapAutenticacionEndpoints();
app.MapUsuarioEndpoints();

app.Run();
