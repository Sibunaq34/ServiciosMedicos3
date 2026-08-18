namespace MicroServicioOferentes.Entities;

public sealed class OferentesPuestoResponse
{
    public IReadOnlyList<OferentePuesto> Data { get; set; } = [];

    public PaginacionMeta Meta { get; set; } = new();
}
