using MicroServiciosEmpleados.Entities;
using MicroServiciosEmpleados.Services;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicioEmpleados;

public static class EmpleadosEndPoint
{
    public static void MapEmpleadosEndpoints(this IEndpointRouteBuilder rutas)
    {
        rutas.MapPost("/api/Empleados", async (
            [FromBody] EntradaRegistrarEmpleado solicitud,
            [FromServices] IEmpleados empleadosService) =>
        {
            if (solicitud is null
                || solicitud.IdOferente <= 0
                || string.IsNullOrWhiteSpace(solicitud.CodigoPuesto)
                || solicitud.IdUsuario <= 0
                || (solicitud.IdJefatura.HasValue && solicitud.IdJefatura.Value <= 0))
            {
                return Results.BadRequest(new
                {
                    mensaje = "Los datos para registrar el empleado son inválidos."
                });
            }

            try
            {
                var resultado = await empleadosService.RegistrarEmpleado(solicitud);

                return resultado switch
                {
                    "" => Results.Created("/api/Empleados", new
                    {
                        mensaje = "Empleado registrado correctamente.",
                        solicitud.IdOferente,
                        CodigoPuesto = solicitud.CodigoPuesto.Trim()
                    }),
                    "OFFERER_NOT_FOUND" => Results.NotFound(new
                    {
                        mensaje = "El oferente indicado no existe."
                    }),
                    "POSITION_NOT_FOUND" => Results.NotFound(new
                    {
                        mensaje = "El puesto indicado no existe."
                    }),
                    "MANAGER_NOT_FOUND" => Results.NotFound(new
                    {
                        mensaje = "La jefatura indicada no existe."
                    }),
                    "EMPLOYEE_ALREADY_EXISTS" => Results.BadRequest(new
                    {
                        mensaje = "El oferente ya está registrado como empleado."
                    }),
                    _ => Results.Json(new
                    {
                        mensaje = "No fue posible registrar el empleado."
                    }, statusCode: StatusCodes.Status500InternalServerError)
                };
            }
            catch
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Error",
                    detail: "No fue posible registrar el empleado.");
            }
        })
        .WithName("RegistrarEmpleado")
        .WithOpenApi();
    }
}
