using Dapper;
using MicroServiciosEmpleados.Entities;
using System.Data;


namespace MicroServiciosEmpleados.Repository
{
    public class EmpleadosRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public EmpleadosRepository(
            IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<OferenteCombo>>
            ListarOferentes()
        {
            using (var connection =
                _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<OferenteCombo>(
                    "SP_ListarOferentes",
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<Puesto>>
            ListarPuestos()
        {
            using (var connection =
                _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Puesto>(
                    "SP_ListarPuestos",
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<EmpleadoCombo>>
            ListarEmpleados()
        {
            using (var connection =
                _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<EmpleadoCombo>(
                    "SP_ListarEmpleados",
                    commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool>
            ContratarEmpleado(
            EmpleadoContratacion empleado)
        {
            using (var connection =
                _dbConnectionFactory.CreateConnection())
            {
                var filas =
                    await connection.ExecuteAsync(
                        "SP_ContratarEmpleado",
                        new
                        {
                            pIdOferente = empleado.IdOferente,
                            pIdPuesto = empleado.IdPuesto,
                            pIdJefatura = empleado.IdJefatura
                        },
                        commandType:
                        CommandType.StoredProcedure);

                return filas > 0;
            }
        }

        public async Task<string> ValidarContratacion(
            EntradaRegistrarEmpleado solicitud)
        {
            using (var connection =
                _dbConnectionFactory.CreateConnection())
            {
                if (await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM oferentes WHERE id_oferente = @Id",
                    new { Id = solicitud.IdOferente }) == 0)
                    return "OFFERER_NOT_FOUND";

                if (await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM empleados WHERE id_oferente = @Id",
                    new { Id = solicitud.IdOferente }) > 0)
                    return "EMPLOYEE_ALREADY_EXISTS";

                if (await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM puestos WHERE codigo_puesto = @Codigo",
                    new { Codigo = solicitud.CodigoPuesto }) == 0)
                    return "POSITION_NOT_FOUND";

                if (solicitud.IdJefatura.HasValue &&
                    await connection.ExecuteScalarAsync<int>(
                        "SELECT COUNT(1) FROM empleados WHERE id_empleado = @Id",
                        new { Id = solicitud.IdJefatura.Value }) == 0)
                    return "MANAGER_NOT_FOUND";

                return string.Empty;
            }
        }

        public async Task<bool> OferenteEsEmpleado(int idOferente)
        {
            using (var connection =
                _dbConnectionFactory.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(
                    "SELECT COUNT(1) FROM empleados WHERE id_oferente = @Id",
                    new { Id = idOferente }) > 0;
            }
        }

        public async Task<bool> RegistrarEmpleado(
            EntradaRegistrarEmpleado solicitud)
        {
            using (var connection =
                _dbConnectionFactory.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var idPuesto = await connection.ExecuteScalarAsync<int>(
                            "SELECT id_puesto FROM puestos " +
                            "WHERE codigo_puesto = @Codigo LIMIT 1",
                            new { Codigo = solicitud.CodigoPuesto },
                            transaction);

                        var filas = await connection.ExecuteAsync(
                            "SP_ContratarEmpleado",
                            new
                            {
                                pIdOferente = solicitud.IdOferente,
                                pIdPuesto = idPuesto,
                                pIdJefatura = solicitud.IdJefatura
                            },
                            transaction,
                            commandType: CommandType.StoredProcedure);

                        if (filas <= 0)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        // Bitácora desactivada temporalmente: comentar la inserción para evitar registros
                        // que puedan causar errores de versión/validación en entornos de despliegue.
                        // Si se desea reactivar, descomentar el bloque y confirmar que descripcionAccion
                        // cumple CHECK (json_valid(descripcionAccion)).
                        /*
                        await connection.ExecuteAsync(
                            "INSERT INTO bitacoras " +
                            "(id_usuario, accion, descripcionAccion) " +
                            "VALUES (@IdUsuario, @Accion, @Descripcion)",
                            new
                            {
                                solicitud.IdUsuario,
                                Accion = "REGISTRAR_EMPLEADO",
                                Descripcion = JsonSerializer.Serialize(new
                                {
                                    accion = "REGISTRAR_EMPLEADO",
                                    idOferente = solicitud.IdOferente,
                                    mensaje = $"Oferente {solicitud.IdOferente} convertido en empleado."
                                })
                            },
                            transaction);
                        */

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}