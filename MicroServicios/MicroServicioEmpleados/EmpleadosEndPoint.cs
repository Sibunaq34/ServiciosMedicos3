using System.ComponentModel.DataAnnotations;
using MicroServicioEmpleados.Entities;
using MicroServiciosEmpleados.Services;
using Microsoft.AspNetCore.Mvc;

namespace MicroServiciosEmpleados;

public static class EmpleadosEndPoint
{
    public static void MapEmpleadosEndpoints(this IEndpointRouteBuilder rutas)
    {
        var grupo = rutas
            .MapGroup("/Empleados")
            .WithTags("Empleados")
            .RequireCors("ReactDev");

        grupo.MapPost("/", async ([FromServices] IEmpleados empleadosService, [FromBody] CrearEmpleadoRequest? request) =>
        {
            if (request is null)
            {
                return Results.BadRequest(new { mensaje = "El body de la solicitud es obligatorio." });
            }

            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, true))
            {
                return Results.BadRequest(new { mensaje = "Hay campos obligatorios inválidos.", errores = validationResults.Select(x => x.ErrorMessage) });
            }

            try
            {
                var resultado = await empleadosService.CrearEmpleado(request);

                if (resultado.Empleado is not null)
                {
                    return Results.Created($"/Empleados/{resultado.Empleado.IdEmpleado}", resultado.Empleado);
                }

                if (resultado.Error == "El oferente ya fue convertido en empleado.")
                {
                    return Results.Conflict(new { mensaje = resultado.Error });
                }

                return Results.NotFound(new { mensaje = resultado.Error });
            }
            catch (Exception)
            {
                return Results.Problem(statusCode: 500, title: "Error", detail: "Error al crear el empleado.");
            }
        })
        .WithName("CrearEmpleado")
        .WithOpenApi();
    }
}
