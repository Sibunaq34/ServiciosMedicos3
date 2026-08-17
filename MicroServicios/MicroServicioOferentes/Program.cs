using MicroServicioOferentes.Entities;
using MicroServicioOferentes.Services;
using Servicios_Medicos.Repository;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("ReactApp");

app.MapGet("/api/v1/puestos/{codigoPuesto}/oferentes", async (
    string codigoPuesto,
    int? page,
    int? pageSize,
    IOferentePuestoService service,
    ILogger<Program> logger) =>
{
    const int maxPageSize = 100;
    var pageNumber = page ?? 1;
    var pageLength = pageSize ?? 10;

    if (string.IsNullOrWhiteSpace(codigoPuesto))
    {
        return Results.BadRequest(CreateError("VALIDATION_ERROR", "El codigo de puesto es requerido."));
    }

    if (pageNumber < 1)
    {
        return Results.BadRequest(CreateError("VALIDATION_ERROR", "La pagina debe ser mayor o igual a 1."));
    }

    if (pageLength < 1 || pageLength > maxPageSize)
    {
        return Results.BadRequest(CreateError("VALIDATION_ERROR", $"El tamano de pagina debe estar entre 1 y {maxPageSize}."));
    }

    try
    {
        var codigoNormalizado = codigoPuesto.Trim();
        var existePuesto = await service.ExistePuestoActivoAsync(codigoNormalizado);

        if (!existePuesto)
        {
            return Results.NotFound(CreateError("NOT_FOUND", "No existe un puesto activo con el codigo indicado."));
        }

        var response = await service.ListarPorPuestoAsync(codigoNormalizado, pageNumber, pageLength);
        return Results.Ok(response);
    }
    catch (Exception exception)
    {
        logger.LogError(exception, "Error al consultar oferentes por puesto.");
        return Results.Json(
            CreateError("INTERNAL_ERROR", "No fue posible consultar los oferentes del puesto."),
            statusCode: StatusCodes.Status500InternalServerError);
    }
})
.WithName("ListarOferentesPorPuesto")
.Produces<OferentesPuestoResponse>(StatusCodes.Status200OK)
.Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
.Produces<ErrorResponse>(StatusCodes.Status404NotFound)
.Produces<ErrorResponse>(StatusCodes.Status500InternalServerError);

app.Run();

static ErrorResponse CreateError(string code, string message)
{
    return new ErrorResponse
    {
        Error = new ErrorDetail
        {
            Code = code,
            Message = message
        }
    };
}
