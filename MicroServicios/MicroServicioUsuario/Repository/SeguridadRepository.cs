using System.Data;
using System.Data.Common;
using Dapper;
using MicroServicioUsuario.Entities;

namespace MicroServicioUsuario.Repository;

public sealed class SeguridadRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public SeguridadRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<UsuarioEntidad?> ObtenerUsuarioAsync(string usuario)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("pUsuario", usuario, DbType.String);

        return await connection.QueryFirstOrDefaultAsync<UsuarioEntidad>(
            "ValidarUsuario",
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> RegistrarIntentoFallidoAsync(int idUsuario)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("pIdUsuario", idUsuario, DbType.Int32);

        return await connection.QuerySingleAsync<int>(
            "RegistrarIntentoFallido",
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task ReiniciarIntentosFallidosAsync(int idUsuario)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("pIdUsuario", idUsuario, DbType.Int32);

        await connection.ExecuteAsync(
            "ReiniciarIntentosFallidos",
            parameters,
            commandType: CommandType.StoredProcedure);
    }
}
