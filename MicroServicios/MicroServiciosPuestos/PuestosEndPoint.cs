using MicroServicioPuestos.Entities;
using MicroServicioPuestos.Services;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicioPuestos
{
    public static class PuestosEndPoints
    {
        public static void MapPuestosEndpoints(this IEndpointRouteBuilder rutas)
        {
            var grupo = rutas
                .MapGroup("/Puestos") // Indica que todos url empieza con la ruta ejemplo localhost:8080/api/Puesto/Crear
                .WithTags(nameof(Puestos))
                .RequireCors("ReactDev");

            grupo.MapGet("/ListaPuestos/{pagina}", async([FromServices] IPuestos puestoservice, int pagina) =>
            {
                try {  
                    var puestos = await puestoservice.ListarPuestos(pagina);

                    if (puestos == null)
                    {
                        return Results.NotFound(new
                        {
                            mensaje = "No se encontraron puestos"
                        });
                    }
                    return Results.Ok(puestos);
                }
                catch (Exception)
                { 
                    return Results.Problem(statusCode: 500,
                        title: "Error",
                        detail: "Error al listar los puestos");
                }
            })
            .WithName("ListarPuestos")
            .WithOpenApi();
        }

    }
}
