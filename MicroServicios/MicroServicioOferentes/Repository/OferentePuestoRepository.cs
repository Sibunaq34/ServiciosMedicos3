using System.Data;
using Dapper;
using MicroServicioOferentes.Entities;

namespace MicroServicioOferentes.Repository;

public sealed class OferentePuestoRepository : IOferentePuestoRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public OferentePuestoRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> ExistePuestoActivoAsync(string codigoPuesto)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var total = await connection.ExecuteScalarAsync<int>(
            """
            SELECT COUNT(1)
            FROM puestos
            WHERE codigo_puesto = @CodigoPuesto
              AND activo = 1;
            """,
            new { CodigoPuesto = codigoPuesto });

        return total > 0;
    }

    public async Task<(IReadOnlyList<OferentePuesto> Oferentes, int Total)> ListarPorPuestoAsync(
        string codigoPuesto,
        int page,
        int pageSize)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        using var result = await connection.QueryMultipleAsync(
            "sp_ObtenerOferentesPorPuesto",
            new
            {
                pCodigoPuesto = codigoPuesto,
                pPage = page,
                pPageSize = pageSize
            },
            commandType: CommandType.StoredProcedure);

        var totalResult = await result.ReadSingleAsync<TotalResult>();
        var oferentes = (await result.ReadAsync<OferentePuesto>()).AsList();

        return (oferentes, totalResult.Total);
    }

    private sealed class TotalResult
    {
        public int Total { get; set; }
    }
}
