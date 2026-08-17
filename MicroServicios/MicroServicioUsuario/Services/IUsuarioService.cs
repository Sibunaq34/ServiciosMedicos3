namespace MicroServicioUsuario.Services;

public interface IUsuarioService
{
    Task<CrearUsuarioResult> CrearUsuarioAsync(CrearUsuarioCommand command);
}

public sealed record CrearUsuarioCommand(
    string Usuario,
    string NombreCompleto,
    string Correo,
    string Contrasena,
    int IdRol,
    string Estado);

public sealed class CrearUsuarioResult
{
    public bool IsSuccess { get; init; }
    public bool IsInvalid { get; init; }
    public bool IsConflict { get; init; }
    public bool IsTechnicalError { get; init; }
    public string Mensaje { get; init; } = string.Empty;
    public UsuarioCreadoSeguro? Usuario { get; init; }
}

public sealed record UsuarioCreadoSeguro(
    int IdUsuario,
    string Usuario,
    string? NombreCompleto,
    string Correo,
    int IdRol,
    string? NombreRol,
    string Estado);
