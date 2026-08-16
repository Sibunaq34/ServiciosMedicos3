namespace MicroServicioUsuario.Entities;

public sealed class UsuarioEntidad
{
    public int IdUsuario { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public string? NombreCompleto { get; set; }
    public string PasswordCifrada { get; set; } = string.Empty;
    public int IdRol { get; set; }
    public string? NombreRol { get; set; }
    public bool Activo { get; set; }
    public string Estado { get; set; } = string.Empty;
    public int IntentosFallidos { get; set; }
}
