using MicroServicioUsuario.Services;

namespace MicroServicioUsuario;

public static class UsuarioEndPoint
{
    public static void MapUsuarioEndpoints(this WebApplication app)
    {
        app.MapPost("/api/usuarios", async (CrearUsuarioRequest request, IUsuarioService usuarioService) =>
        {
            if (request is null)
            {
                return Results.BadRequest(new { mensaje = "Los datos del usuario son requeridos." });
            }

            var result = await usuarioService.CrearUsuarioAsync(new CrearUsuarioCommand(
                request.Usuario,
                request.NombreCompleto,
                request.Correo,
                request.Contrasena,
                request.IdRol,
                request.Estado));

            if (result.IsInvalid)
                return Results.BadRequest(new { mensaje = result.Mensaje });
            if (result.IsConflict)
                return Results.Conflict(new { mensaje = result.Mensaje });
            if (result.IsTechnicalError || result.Usuario is null)
                return Results.Json(new { mensaje = "No fue posible crear el usuario." }, statusCode: 500);

            return Results.Created($"/api/usuarios/{result.Usuario.IdUsuario}", result.Usuario);
        })
        .WithName("CrearUsuario");
    }

    private sealed record CrearUsuarioRequest(
        string Usuario,
        string NombreCompleto,
        string Correo,
        string Contrasena,
        int IdRol,
        string Estado);
}
