using MicroServicioEmpleados;
using MicroServiciosEmpleados.Repository;
using MicroServiciosEmpleados.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<EmpleadosRepository>();
builder.Services.AddScoped<IEmpleados, EmpleadosService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapEmpleadosEndpoints();

app.Run();
