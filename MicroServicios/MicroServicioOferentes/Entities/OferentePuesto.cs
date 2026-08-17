namespace MicroServicioOferentes.Entities;

public sealed class OferentePuesto
{
    public int IdOferente { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Identificacion { get; set; } = string.Empty;
}
