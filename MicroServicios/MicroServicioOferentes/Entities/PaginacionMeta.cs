namespace MicroServicioOferentes.Entities;

public sealed class PaginacionMeta
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int Total { get; set; }

    public int TotalPages { get; set; }
}
