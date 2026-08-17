using System.Security.Cryptography;
using System.Text;

namespace MicroServicioUsuario.Services;

public sealed class PasswordEncryptionService : IPasswordEncryptionService
{
    private const string GcmPrefix = "GCM:";
    private const int GcmNonceLength = 12;
    private const int GcmTagLength = 16;
    private const int CbcIvLength = 16;

    private readonly IConfiguration _configuration;

    public PasswordEncryptionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Encrypt(string password)
    {
        var nonce = RandomNumberGenerator.GetBytes(GcmNonceLength);
        var plaintext = Encoding.UTF8.GetBytes(password);
        var ciphertext = new byte[plaintext.Length];
        var tag = new byte[GcmTagLength];

        using var aesGcm = new AesGcm(GetAesKey(), GcmTagLength);
        aesGcm.Encrypt(nonce, plaintext, ciphertext, tag);

        var payload = new byte[nonce.Length + ciphertext.Length + tag.Length];
        Buffer.BlockCopy(nonce, 0, payload, 0, nonce.Length);
        Buffer.BlockCopy(ciphertext, 0, payload, nonce.Length, ciphertext.Length);
        Buffer.BlockCopy(tag, 0, payload, nonce.Length + ciphertext.Length, tag.Length);

        return GcmPrefix + Convert.ToBase64String(payload);
    }

    public string Decrypt(string encryptedPassword)
    {
        if (string.IsNullOrWhiteSpace(encryptedPassword))
        {
            throw new InvalidOperationException("La contraseña almacenada es inválida.");
        }

        return IsGcmFormat(encryptedPassword)
            ? DecryptGcm(encryptedPassword)
            : DecryptLegacyCbc(encryptedPassword);
    }

    public bool IsGcmFormat(string encryptedPassword)
    {
        return encryptedPassword.StartsWith(GcmPrefix, StringComparison.OrdinalIgnoreCase);
    }

    private string DecryptGcm(string encryptedPassword)
    {
        var payload = Convert.FromBase64String(encryptedPassword[GcmPrefix.Length..]);
        if (payload.Length < GcmNonceLength + GcmTagLength)
        {
            throw new InvalidOperationException("El valor de contraseña cifrada no cumple el formato esperado.");
        }

        var ciphertextLength = payload.Length - GcmNonceLength - GcmTagLength;
        var nonce = payload.AsSpan(0, GcmNonceLength).ToArray();
        var ciphertext = payload.AsSpan(GcmNonceLength, ciphertextLength).ToArray();
        var tag = payload.AsSpan(payload.Length - GcmTagLength, GcmTagLength).ToArray();
        var plaintext = new byte[ciphertext.Length];

        using var aesGcm = new AesGcm(GetAesKey(), GcmTagLength);
        aesGcm.Decrypt(nonce, ciphertext, tag, plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }

    private string DecryptLegacyCbc(string encryptedPassword)
    {
        var payload = Convert.FromBase64String(encryptedPassword);
        if (payload.Length <= CbcIvLength)
        {
            throw new InvalidOperationException("El valor de contraseña cifrada no cumple el formato esperado.");
        }

        using var aes = Aes.Create();
        aes.Key = GetAesKey();
        aes.IV = payload.AsSpan(0, CbcIvLength).ToArray();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        var ciphertext = payload.AsSpan(CbcIvLength).ToArray();
        var plaintext = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
        return Encoding.UTF8.GetString(plaintext);
    }

    private byte[] GetAesKey()
    {
        var configuredKey = _configuration["Security:AesKey"];
        if (string.IsNullOrWhiteSpace(configuredKey))
        {
            throw new InvalidOperationException("Security:AesKey no está configurado.");
        }

        var keyBytes = Encoding.UTF8.GetBytes(configuredKey);
        if (keyBytes.Length != 32)
        {
            throw new InvalidOperationException("Security:AesKey debe tener 32 bytes.");
        }

        return keyBytes;
    }
}
