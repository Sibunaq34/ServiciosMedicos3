using MicroServicioOferentes;
using MicroServicioOferentes.Repository;
using MicroServicioOferentes.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IOferentePuestoRepository, OferentePuestoRepository>();
builder.Services.AddScoped<IOferentePuestoService, OferentePuestoService>();
builder.Services.AddScoped<DetalleOferenteRepository>();
builder.Services.AddScoped<IDetalleOferente, DetalleOferenteService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("ReactApp");

app.MapOferentesEndpoints();

app.Run();
