using MicroServicioOferentes.Entities;
using MicroServicioOferentes.Services;

namespace MicroServicioOferentes;

public static class OferentesEndPoint
{
    public static void MapOferentesEndpoints(this IEndpointRouteBuilder rutas)
    {

        rutas.MapGet("/Oferentes/{idOferente}/detalle", async (
        int idOferente,
        IDetalleOferente service,
        ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger(
                    "MicroServicioOferentes.OferentesEndPoint");

                try
                {
                    var response = await service.ObtenerDetalleAsync(idOferente);

                    if (!response.Exito)
                    {
                        if (idOferente <= 0)
                        {
                            return Results.BadRequest(response);
                        }

                        if (response.Datos == null)
                        {
                            return Results.NotFound(response);
                        }

                        return Results.Json(
                            response,
                            statusCode: StatusCodes.Status500InternalServerError);
                    }

                    return Results.Ok(response);
                }
                catch (Exception exception)
                {
                    logger.LogError(
                        exception,
                        "Error al consultar el detalle del oferente {IdOferente}.",
                        idOferente);

                    return Results.Json(
                        new ResultadoDetalleOferente
                        {
                            Exito = false,
                            Mensaje = "No fue posible consultar el detalle del oferente.",
                            Datos = null
                        },
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
        .WithName("ObtenerDetalleOferente")
        .Produces<ResultadoDetalleOferente>(StatusCodes.Status200OK)
        .Produces<ResultadoDetalleOferente>(StatusCodes.Status400BadRequest)
        .Produces<ResultadoDetalleOferente>(StatusCodes.Status404NotFound)
        .WithOpenApi()
        .RequireCors("ReactApp");

        rutas.MapGet("/Puestos/{codigoPuesto}/oferentes", async (
            string codigoPuesto,
            int? page,
            int? pageSize,
            IOferentePuestoService service,
            ILoggerFactory loggerFactory) =>
        {
            var logger = loggerFactory.CreateLogger("MicroServicioOferentes.OferentesEndPoint");
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
        .Produces<ErrorResponse>(StatusCodes.Status500InternalServerError)
        .WithOpenApi()
        .RequireCors("ReactApp");
    }

    private static ErrorResponse CreateError(string code, string message)
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
}