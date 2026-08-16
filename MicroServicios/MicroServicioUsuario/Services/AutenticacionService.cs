using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MicroServicioUsuario.Entities;
using MicroServicioUsuario.Repository;

namespace MicroServicioUsuario.Services;

public sealed class AutenticacionService : IAutenticacionService
{
    private readonly SeguridadRepository _seguridadRepository;
    private readonly IConfiguration _configuration;

    public AutenticacionService(SeguridadRepository seguridadRepository, IConfiguration configuration)
    {
        _seguridadRepository = seguridadRepository;
        _configuration = configuration;
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
            var contraseñaDesencriptada = DesencriptarPassword(contraseñaAlmacenada);

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

    private string DesencriptarPassword(string passwordCifrada)
    {
        if (string.IsNullOrWhiteSpace(passwordCifrada))
        {
            throw new InvalidOperationException("La contraseña almacenada es inválida.");
        }

        var key = ObtenerClaveAes();
        var rawDatos = Convert.FromBase64String(passwordCifrada);

        const int nonceLength = 12;
        const int tagLength = 16;

        if (rawDatos.Length < nonceLength + tagLength)
        {
            throw new InvalidOperationException("El valor de contraseña cifrada no cumple el formato esperado.");
        }

        var nonce = rawDatos.AsSpan(0, nonceLength).ToArray();
        var tag = rawDatos.AsSpan(nonceLength, tagLength).ToArray();
        var ciphertext = rawDatos.AsSpan(nonceLength + tagLength).ToArray();
        var plaintext = new byte[ciphertext.Length];

        using var aesGcm = new AesGcm(key, tagLength);
        aesGcm.Decrypt(nonce, ciphertext, tag, plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }

    private byte[] ObtenerClaveAes()
    {
        var claveConfigurada = _configuration["Security:AesKey"];
        if (string.IsNullOrWhiteSpace(claveConfigurada))
        {
            throw new InvalidOperationException("Security:AesKey no está configurado.");
        }

        var keyBytes = TryDecodeBase64(claveConfigurada);
        if (keyBytes is null)
        {
            keyBytes = Encoding.UTF8.GetBytes(claveConfigurada);
        }

        if (keyBytes.Length != 32)
        {
            throw new InvalidOperationException("Security:AesKey debe tener 32 bytes.");
        }

        return keyBytes;
    }

    private static byte[]? TryDecodeBase64(string value)
    {
        try
        {
            return Convert.FromBase64String(value);
        }
        catch
        {
            return null;
        }
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
