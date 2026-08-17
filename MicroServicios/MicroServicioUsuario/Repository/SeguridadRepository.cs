using System.Data;
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

    public async Task ActualizarPasswordCifradaUsuarioAsync(int idUsuario, string passwordCifrada)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("pIdUsuario", idUsuario, DbType.Int32);
        parameters.Add("pPasswordCifrada", passwordCifrada, DbType.String);

        await connection.ExecuteAsync(
            "ActualizarPasswordCifradaUsuario",
            parameters,
            commandType: CommandType.StoredProcedure);
    }

    public async Task CrearUsuarioAsync(
        string usuario,
        string nombreCompleto,
        string correo,
        string passwordCifrada,
        string estado,
        int idRol)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        var parameters = new DynamicParameters();
        parameters.Add("p_usuario", usuario, DbType.String);
        parameters.Add("p_nombre_completo", nombreCompleto, DbType.String);
        parameters.Add("p_correo", correo, DbType.String);
        parameters.Add("p_contrasena", passwordCifrada, DbType.String);
        parameters.Add("p_estado", estado, DbType.String);
        parameters.Add("p_id_rol", idRol, DbType.Int32);

        await connection.ExecuteAsync(
            "sp_Usuarios_Crear",
            parameters,
            commandType: CommandType.StoredProcedure);
    }
}
