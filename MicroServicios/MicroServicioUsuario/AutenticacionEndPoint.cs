using MicroServicioUsuario.Services;

namespace MicroServicioUsuario;

public static class AutenticacionEndPoint
{
    public static void MapAutenticacionEndpoints(this WebApplication app)
    {
        app.MapPost("/api/autenticacion/login", async (LoginRequest request, IAutenticacionService autenticacionService) =>
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Usuario) || string.IsNullOrWhiteSpace(request.Contrasena))
            {
                return Results.BadRequest(new { mensaje = "Usuario y contraseña son requeridos." });
            }

            var resultado = await autenticacionService.AuthenticateAsync(request.Usuario.Trim(), request.Contrasena);

            if (resultado.IsTechnicalError)
            {
                return Results.Json(new { mensaje = "Error de comunicación." }, statusCode: 500);
            }

            if (resultado.IsForbidden)
            {
                return Results.Json(new { mensaje = "El usuario no tiene acceso al sistema." }, statusCode: 403);
            }

            if (!resultado.IsSuccess)
            {
                return Results.Json(new { mensaje = "Usuario y/o contraseña incorrectos." }, statusCode: 401);
            }

            return Results.Ok(new
            {
                token = resultado.Token,
                usuario = resultado.Usuario
            });
        })
        .WithName("Login");
    }

    private sealed record LoginRequest(string Usuario, string Contrasena);
}
