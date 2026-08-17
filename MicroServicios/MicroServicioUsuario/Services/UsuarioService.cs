using System.Net.Mail;
using System.Security.Cryptography;
using MicroServicioUsuario.Repository;

namespace MicroServicioUsuario.Services;

public sealed class UsuarioService : IUsuarioService
{
    private readonly SeguridadRepository _seguridadRepository;
    private readonly IPasswordEncryptionService _passwordEncryptionService;

    public UsuarioService(
        SeguridadRepository seguridadRepository,
        IPasswordEncryptionService passwordEncryptionService)
    {
        _seguridadRepository = seguridadRepository;
        _passwordEncryptionService = passwordEncryptionService;
    }

    public async Task<CrearUsuarioResult> CrearUsuarioAsync(CrearUsuarioCommand command)
    {
        var validationMessage = Validate(command);
        if (validationMessage is not null)
        {
            return Invalid(validationMessage);
        }

        var usuario = command.Usuario.Trim();
        var nombreCompleto = command.NombreCompleto.Trim();
        var correo = command.Correo.Trim();
        var estado = NormalizeStatus(command.Estado);

        try
        {
            if (await _seguridadRepository.ObtenerUsuarioAsync(usuario) is not null)
            {
                return new CrearUsuarioResult
                {
                    IsConflict = true,
                    Mensaje = "El nombre de usuario ya existe."
                };
            }

            var passwordCifrada = _passwordEncryptionService.Encrypt(command.Contrasena);
            await _seguridadRepository.CrearUsuarioAsync(
                usuario,
                nombreCompleto,
                correo,
                passwordCifrada,
                estado,
                command.IdRol);

            var usuarioCreado = await _seguridadRepository.ObtenerUsuarioAsync(usuario);
            if (usuarioCreado is null)
            {
                return TechnicalError();
            }

            return new CrearUsuarioResult
            {
                IsSuccess = true,
                Usuario = new UsuarioCreadoSeguro(
                    usuarioCreado.IdUsuario,
                    usuarioCreado.Usuario,
                    usuarioCreado.NombreCompleto,
                    correo,
                    usuarioCreado.IdRol,
                    usuarioCreado.NombreRol,
                    usuarioCreado.Estado)
            };
        }
        catch (InvalidOperationException)
        {
            return TechnicalError();
        }
        catch (CryptographicException)
        {
            return TechnicalError();
        }
        catch (Exception)
        {
            return TechnicalError();
        }
    }

    private static string? Validate(CrearUsuarioCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Usuario))
            return "El usuario es requerido.";
        if (command.Usuario.Trim().Length > 50)
            return "El usuario no puede superar 50 caracteres.";
        if (string.IsNullOrWhiteSpace(command.NombreCompleto))
            return "El nombre completo es requerido.";
        if (command.NombreCompleto.Trim().Length > 150)
            return "El nombre completo no puede superar 150 caracteres.";
        if (string.IsNullOrWhiteSpace(command.Correo))
            return "El correo es requerido.";

        var correo = command.Correo.Trim();
        if (correo.Length > 150)
            return "El correo no puede superar 150 caracteres.";
        if (!MailAddress.TryCreate(correo, out var parsedEmail)
            || !string.Equals(parsedEmail.Address, correo, StringComparison.OrdinalIgnoreCase))
            return "El formato del correo no es válido.";
        if (string.IsNullOrWhiteSpace(command.Contrasena))
            return "La contraseña es requerida.";
        if (command.Contrasena.Length < 8)
            return "La contraseña debe tener al menos 8 caracteres.";
        if (!command.Contrasena.Any(char.IsUpper))
            return "La contraseña debe contener al menos una mayúscula.";
        if (!command.Contrasena.Any(char.IsLower))
            return "La contraseña debe contener al menos una minúscula.";
        if (!command.Contrasena.Any(char.IsDigit))
            return "La contraseña debe contener al menos un número.";
        if (!command.Contrasena.Any(character => !char.IsLetterOrDigit(character)))
            return "La contraseña debe contener al menos un carácter especial.";
        if (command.IdRol <= 0)
            return "El rol debe ser mayor a cero.";
        if (string.IsNullOrWhiteSpace(command.Estado)
            || (!command.Estado.Trim().Equals("Activo", StringComparison.OrdinalIgnoreCase)
                && !command.Estado.Trim().Equals("Inactivo", StringComparison.OrdinalIgnoreCase)))
            return "El estado debe ser Activo o Inactivo.";

        return null;
    }

    private static string NormalizeStatus(string status)
    {
        return status.Trim().Equals("Activo", StringComparison.OrdinalIgnoreCase)
            ? "Activo"
            : "Inactivo";
    }

    private static CrearUsuarioResult Invalid(string message) => new()
    {
        IsInvalid = true,
        Mensaje = message
    };

    private static CrearUsuarioResult TechnicalError() => new()
    {
        IsTechnicalError = true,
        Mensaje = "No fue posible crear el usuario."
    };
}
