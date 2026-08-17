using System.Data;
using Dapper;
using MicroServicioEmpleados.Entities;

namespace MicroServiciosEmpleados.Repository;

public class EmpleadosRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public EmpleadosRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<CrearEmpleadoRepositoryResult> CrearEmpleado(CrearEmpleadoRequest request)
    {
        using var connection = _dbConnectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction(IsolationLevel.Serializable);

        try
        {
            var oferenteExiste = await connection.ExecuteScalarAsync<int?>(new CommandDefinition(
                "SELECT id_oferente FROM oferentes WHERE id_oferente = @IdOferente FOR UPDATE",
                new { request.IdOferente }, transaction, cancellationToken: CancellationToken.None));

            if (!oferenteExiste.HasValue)
            {
                transaction.Rollback();
                return CrearEmpleadoRepositoryResult.OferenteNoEncontrado();
            }

            var yaEsEmpleado = await connection.ExecuteScalarAsync<int?>(new CommandDefinition(
                "SELECT id_empleado FROM empleados WHERE id_oferente = @IdOferente LIMIT 1 FOR UPDATE",
                new { request.IdOferente }, transaction, cancellationToken: CancellationToken.None));

            if (yaEsEmpleado.HasValue)
            {
                transaction.Rollback();
                return CrearEmpleadoRepositoryResult.OferenteYaConvertido();
            }

            var puestoExiste = await connection.ExecuteScalarAsync<int?>(new CommandDefinition(
                "SELECT id_puesto FROM puestos WHERE id_puesto = @IdPuesto",
                new { request.IdPuesto }, transaction, cancellationToken: CancellationToken.None));

            if (!puestoExiste.HasValue)
            {
                transaction.Rollback();
                return CrearEmpleadoRepositoryResult.PuestoNoEncontrado();
            }

            var jefaturaExiste = await connection.ExecuteScalarAsync<int?>(new CommandDefinition(
                "SELECT id_empleado FROM empleados WHERE id_empleado = @IdJefatura",
                new { request.IdJefatura }, transaction, cancellationToken: CancellationToken.None));

            if (!jefaturaExiste.HasValue)
            {
                transaction.Rollback();
                return CrearEmpleadoRepositoryResult.JefaturaNoEncontrada();
            }

            await connection.ExecuteAsync(new CommandDefinition(
                "SP_ContratarEmpleado",
                new { pIdOferente = request.IdOferente, pIdPuesto = request.IdPuesto, pIdJefatura = request.IdJefatura },
                transaction,
                commandType: CommandType.StoredProcedure,
                cancellationToken: CancellationToken.None));

            var empleado = await connection.QuerySingleAsync<EmpleadoResponse>(new CommandDefinition(
                """
                SELECT id_empleado AS IdEmpleado,
                       numero_empleado AS NumeroEmpleado,
                       id_oferente AS IdOferente,
                       id_puesto AS IdPuesto,
                       fecha_contratacion AS FechaContratacion,
                       estado AS Estado
                FROM empleados
                WHERE id_oferente = @IdOferente
                ORDER BY id_empleado DESC
                LIMIT 1
                """,
                new { request.IdOferente }, transaction, cancellationToken: CancellationToken.None));

            transaction.Commit();
            return CrearEmpleadoRepositoryResult.Creado(empleado);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}

public class CrearEmpleadoRepositoryResult
{
    public EmpleadoResponse? Empleado { get; private init; }
    public string? Error { get; private init; }

    public static CrearEmpleadoRepositoryResult Creado(EmpleadoResponse empleado) => new() { Empleado = empleado };
    public static CrearEmpleadoRepositoryResult OferenteNoEncontrado() => new() { Error = "El oferente no existe." };
    public static CrearEmpleadoRepositoryResult OferenteYaConvertido() => new() { Error = "El oferente ya fue convertido en empleado." };
    public static CrearEmpleadoRepositoryResult PuestoNoEncontrado() => new() { Error = "El puesto no existe." };
    public static CrearEmpleadoRepositoryResult JefaturaNoEncontrada() => new() { Error = "La jefatura no existe." };
}
