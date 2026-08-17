namespace MicroServicioUsuario.Services;

public interface IPasswordEncryptionService
{
    string Encrypt(string password);
    string Decrypt(string encryptedPassword);
    bool IsGcmFormat(string encryptedPassword);
}
