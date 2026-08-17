using MicroServicioOferentes.Entities;

namespace Servicios_Medicos.Repository;

public interface IOferentePuestoRepository
{
    Task<bool> ExistePuestoActivoAsync(string codigoPuesto);

    Task<(IReadOnlyList<OferentePuesto> Oferentes, int Total)> ListarPorPuestoAsync(
        string codigoPuesto,
        int page,
        int pageSize);
}
