using MicroServiciosEmpleados;
using MicroServiciosEmpleados.Repository;
using MicroServiciosEmpleados.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<EmpleadosRepository>();
builder.Services.AddScoped<IEmpleados, EmpleadosService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.MapOpenApi();
}

app.UseCors("ReactDev");
app.UseHttpsRedirection();
app.MapEmpleadosEndpoints();

app.Run();
