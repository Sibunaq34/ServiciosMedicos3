using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MicroServicioUsuario.Entities;
using MicroServicioUsuario.Repository;

namespace MicroServicioUsuario.Services;

public sealed class AutenticacionService : IAutenticacionService
{
    private readonly SeguridadRepository _seguridadRepository;
    private readonly IConfiguration _configuration;
    private readonly IPasswordEncryptionService _passwordEncryptionService;

    public AutenticacionService(
        SeguridadRepository seguridadRepository,
        IConfiguration configuration,
        IPasswordEncryptionService passwordEncryptionService)
    {
        _seguridadRepository = seguridadRepository;
        _configuration = configuration;
        _passwordEncryptionService = passwordEncryptionService;
    }

    public async Task<AutenticacionResult> AuthenticateAsync(string usuario, string contrasena)
    {
        if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contrasena))
        {
            return new AutenticacionResult
            {
                IsSuccess = false,
                Mensaje = "Usuario y/o contraseña incorrectos."
            };
        }

        try
        {
            var usuarioModelo = await _seguridadRepository.ObtenerUsuarioAsync(usuario.Trim());
            if (usuarioModelo is null)
            {
                return new AutenticacionResult
                {
                    IsSuccess = false,
                    Mensaje = "Usuario y/o contraseña incorrectos."
                };
            }

            if (CuentaNoActiva(usuarioModelo))
            {
                return new AutenticacionResult
                {
                    IsSuccess = false,
                    IsForbidden = true,
                    Mensaje = "El usuario no tiene acceso al sistema."
                };
            }

            var contraseñaAlmacenada = usuarioModelo.PasswordCifrada;
            var contraseñaDesencriptada = _passwordEncryptionService.Decrypt(contraseñaAlmacenada);

            if (!string.Equals(contraseñaDesencriptada, contrasena, StringComparison.Ordinal))
            {
                var intentos = await _seguridadRepository.RegistrarIntentoFallidoAsync(usuarioModelo.IdUsuario);

                if (intentos >= 3)
                {
                    return new AutenticacionResult
                    {
                        IsSuccess = false,
                        IsForbidden = true,
                        Mensaje = "El usuario no tiene acceso al sistema."
                    };
                }

                return new AutenticacionResult
                {
                    IsSuccess = false,
                    Mensaje = "Usuario y/o contraseña incorrectos."
                };
            }

            await _seguridadRepository.ReiniciarIntentosFallidosAsync(usuarioModelo.IdUsuario);

            if (!_passwordEncryptionService.IsGcmFormat(usuarioModelo.PasswordCifrada))
            {
                var passwordGcm = _passwordEncryptionService.Encrypt(contrasena);
                await _seguridadRepository.ActualizarPasswordCifradaUsuarioAsync(
                    usuarioModelo.IdUsuario,
                    passwordGcm);
            }

            var token = GenerarToken(usuarioModelo);
            var usuarioSeguro = new UsuarioSeguro(
                usuarioModelo.IdUsuario,
                usuarioModelo.Usuario,
                usuarioModelo.NombreCompleto,
                usuarioModelo.IdRol,
                usuarioModelo.NombreRol);

            return new AutenticacionResult
            {
                IsSuccess = true,
                Token = token,
                Usuario = usuarioSeguro
            };
        }
        catch (InvalidOperationException)
        {
            return new AutenticacionResult
            {
                IsTechnicalError = true,
                Mensaje = "Error de comunicación."
            };
        }
        catch (CryptographicException)
        {
            return new AutenticacionResult
            {
                IsTechnicalError = true,
                Mensaje = "Error de comunicación."
            };
        }
        catch (Exception)
        {
            return new AutenticacionResult
            {
                IsTechnicalError = true,
                Mensaje = "Error de comunicación."
            };
        }
    }

    private static bool CuentaNoActiva(UsuarioEntidad usuario)
    {
        return !usuario.Activo
            || string.IsNullOrWhiteSpace(usuario.Estado)
            || !string.Equals(usuario.Estado.Trim(), "Activo", StringComparison.OrdinalIgnoreCase);
    }

    private string GenerarToken(UsuarioEntidad usuario)
    {
        var key = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expirationString = _configuration["Jwt:ExpirationMinutes"];

        if (string.IsNullOrWhiteSpace(key)
            || string.IsNullOrWhiteSpace(issuer)
            || string.IsNullOrWhiteSpace(audience)
            || !int.TryParse(expirationString, out var expirationMinutes)
            || expirationMinutes <= 0)
        {
            throw new InvalidOperationException("Configuración JWT incompleta.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Usuario),
            new Claim("idRol", usuario.IdRol.ToString()),
        };

        if (!string.IsNullOrWhiteSpace(usuario.NombreRol))
        {
            claims.Add(new Claim(ClaimTypes.Role, usuario.NombreRol));
        }

        var tokenDescriptor = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
