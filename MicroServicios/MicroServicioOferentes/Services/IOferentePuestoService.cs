using MicroServicioOferentes.Entities;

namespace MicroServicioOferentes.Services;

public interface IOferentePuestoService
{
    Task<bool> ExistePuestoActivoAsync(string codigoPuesto);

    Task<OferentesPuestoResponse> ListarPorPuestoAsync(string codigoPuesto, int page, int pageSize);
}
