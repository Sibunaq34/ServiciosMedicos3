namespace MicroServicioOferentes.Entities;

public sealed class OferenteNormalizado
{
    public int IdOferente { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Identificacion { get; set; } = string.Empty;

    public string TipoIdentificacion { get; set; } = string.Empty;

    public string FechaNacimiento { get; set; } = string.Empty;

    public IReadOnlyList<string> Correos { get; set; } = [];

    public IReadOnlyList<string> Telefonos { get; set; } = [];

    public PuestoDto Puesto { get; set; } = new();

    public CurriculumDto Curriculum { get; set; } = new();
}

public sealed class PuestoDto
{
    public string CodigoPuesto { get; set; } = string.Empty;

    public string NombrePuesto { get; set; } = string.Empty;
}

public sealed class CurriculumDto
{
    public string NombreArchivo { get; set; } = string.Empty;

    public string Mime { get; set; } = string.Empty;

    public string TamanioFormateado { get; set; } = string.Empty;
}
