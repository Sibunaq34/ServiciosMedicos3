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
    private const string GcmPrefix = "GCM:";
    private const int GcmNonceLength = 12;
    private const int GcmTagLength = 16;
    private const int CbcIvLength = 16;

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

            if (!EsFormatoGcm(usuarioModelo.PasswordCifrada))
            {
                var passwordGcm = EncriptarGcm(contrasena);
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

    private string DesencriptarPassword(string passwordCifrada)
    {
        if (string.IsNullOrWhiteSpace(passwordCifrada))
        {
            throw new InvalidOperationException("La contraseña almacenada es inválida.");
        }

        return EsFormatoGcm(passwordCifrada)
            ? DesencriptarGcm(passwordCifrada)
            : DesencriptarLegacyCbc(passwordCifrada);
    }

    private static bool EsFormatoGcm(string passwordCifrada)
    {
        return passwordCifrada.StartsWith(GcmPrefix, StringComparison.OrdinalIgnoreCase);
    }

    private string DesencriptarGcm(string passwordCifrada)
    {
        var payloadBase64 = passwordCifrada[GcmPrefix.Length..];
        var payload = Convert.FromBase64String(payloadBase64);

        if (payload.Length < GcmNonceLength + GcmTagLength)
        {
            throw new InvalidOperationException("El valor de contraseña cifrada no cumple el formato esperado.");
        }

        var ciphertextLength = payload.Length - GcmNonceLength - GcmTagLength;
        var nonce = payload.AsSpan(0, GcmNonceLength).ToArray();
        var ciphertext = payload.AsSpan(GcmNonceLength, ciphertextLength).ToArray();
        var tag = payload.AsSpan(payload.Length - GcmTagLength, GcmTagLength).ToArray();
        var plaintext = new byte[ciphertext.Length];

        using var aesGcm = new AesGcm(ObtenerClaveAes(), GcmTagLength);
        aesGcm.Decrypt(nonce, ciphertext, tag, plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }

    private string DesencriptarLegacyCbc(string passwordCifrada)
    {
        var payload = Convert.FromBase64String(passwordCifrada);
        if (payload.Length <= CbcIvLength)
        {
            throw new InvalidOperationException("El valor de contraseña cifrada no cumple el formato esperado.");
        }

        var iv = payload.AsSpan(0, CbcIvLength).ToArray();
        var ciphertext = payload.AsSpan(CbcIvLength).ToArray();

        using var aes = Aes.Create();
        aes.Key = ObtenerClaveAes();
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        var plaintext = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
        return Encoding.UTF8.GetString(plaintext);
    }

    private string EncriptarGcm(string password)
    {
        var nonce = RandomNumberGenerator.GetBytes(GcmNonceLength);
        var plaintext = Encoding.UTF8.GetBytes(password);
        var ciphertext = new byte[plaintext.Length];
        var tag = new byte[GcmTagLength];

        using var aesGcm = new AesGcm(ObtenerClaveAes(), GcmTagLength);
        aesGcm.Encrypt(nonce, plaintext, ciphertext, tag);

        var payload = new byte[nonce.Length + ciphertext.Length + tag.Length];
        Buffer.BlockCopy(nonce, 0, payload, 0, nonce.Length);
        Buffer.BlockCopy(ciphertext, 0, payload, nonce.Length, ciphertext.Length);
        Buffer.BlockCopy(tag, 0, payload, nonce.Length + ciphertext.Length, tag.Length);

        return GcmPrefix + Convert.ToBase64String(payload);
    }

    private byte[] ObtenerClaveAes()
    {
        var claveConfigurada = _configuration["Security:AesKey"];
        if (string.IsNullOrWhiteSpace(claveConfigurada))
        {
            throw new InvalidOperationException("Security:AesKey no está configurado.");
        }

        var keyBytes = Encoding.UTF8.GetBytes(claveConfigurada);

        if (keyBytes.Length != 32)
        {
            throw new InvalidOperationException("Security:AesKey debe tener 32 bytes.");
        }

        return keyBytes;
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
