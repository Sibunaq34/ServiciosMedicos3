using MicroServicioOferentes.Entities;
using MicroServicioOferentes.Repository;

namespace MicroServicioOferentes.Services;

public sealed class OferentePuestoService : IOferentePuestoService
{
    private readonly IOferentePuestoRepository _repository;

    public OferentePuestoService(IOferentePuestoRepository repository)
    {
        _repository = repository;
    }

    public Task<bool> ExistePuestoActivoAsync(string codigoPuesto)
    {
        return _repository.ExistePuestoActivoAsync(codigoPuesto);
    }

    public async Task<OferentesPuestoResponse> ListarPorPuestoAsync(string codigoPuesto, int page, int pageSize)
    {
        var (oferentes, total) = await _repository.ListarPorPuestoAsync(codigoPuesto, page, pageSize);

        return new OferentesPuestoResponse
        {
            Data = oferentes,
            Meta = new PaginacionMeta
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                TotalPages = total == 0 ? 0 : (int)Math.Ceiling(total / (double)pageSize)
            }
        };
    }
}
