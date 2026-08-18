namespace MicroServicioUsuario.Services;

public interface IAutenticacionService
{
    Task<AutenticacionResult> AuthenticateAsync(string usuario, string contrasena);
}

public sealed class AutenticacionResult
{
    public bool IsSuccess { get; init; }
    public bool IsForbidden { get; init; }
    public bool IsTechnicalError { get; init; }
    public string Mensaje { get; init; } = string.Empty;
    public string? Token { get; init; }
    public UsuarioSeguro? Usuario { get; init; }
}

public sealed record UsuarioSeguro(int IdUsuario, string Usuario, string? NombreCompleto, int IdRol, string? NombreRol);
