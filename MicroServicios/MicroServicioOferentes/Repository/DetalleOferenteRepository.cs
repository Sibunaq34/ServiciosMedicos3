using Dapper;
using MySql.Data.MySqlClient;
using MicroServicioOferentes.Entities;
using System.Data;

namespace MicroServicioOferentes.Repository
{
    // Persona C - Kenneth
    // Consulta solo el detalle registrado por AUT3 sin modificar la base.
    public class DetalleOferenteRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DetalleOferenteRepository(
            IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<DetalleOferente> ObtenerDetalleAsync(
            int idOferente)
        {
            try
            {
                using (var connection =
                    _dbConnectionFactory.CreateConnection())
                {
                    var detalle =
                        await connection.QuerySingleOrDefaultAsync<DetalleOferente>(
                            @"SELECT
                                o.id_oferente AS IdOferente,
                                p.identificacion AS Identificacion,
                                p.tipo_identificacion AS TipoIdentificacion,
                                p.nombre_comple AS NombreCompleto,
                                p.fecha_naci AS FechaNacimiento
                              FROM oferentes o
                              INNER JOIN personas p ON p.id_persona = o.id_persona
                              WHERE o.id_oferente = @IdOferente;",
                            new { IdOferente = idOferente });

                    if (detalle == null)
                    {
                        return null;
                    }

                    detalle.Correos =
                        (await connection.QueryAsync<string>(
                            @"SELECT correo
                              FROM oferente_correo
                              WHERE id_oferente = @IdOferente
                              ORDER BY id_of_correo;",
                            new { IdOferente = idOferente }))
                        .ToList();

                    detalle.Telefonos =
                        (await connection.QueryAsync<string>(
                            @"SELECT telefono
                              FROM oferente_telf
                              WHERE id_oferente = @IdOferente
                              ORDER BY id_of_telefono;",
                            new { IdOferente = idOferente }))
                        .ToList();

                    var postulacion =
                        await connection.QuerySingleOrDefaultAsync<PostulacionAut3Row>(
                            @"SELECT
                                p.codigo_puesto AS CodigoPuesto,
                                p.nombre_puesto AS NombrePuesto,
                                op.nombre_curriculum AS NombreArchivo,
                                op.mime_curriculum AS Mime,
                                op.tamanio_curriculum AS Tamanio
                              FROM oferente_puesto op
                              INNER JOIN puestos p ON p.id_puesto = op.id_puesto
                              WHERE op.id_oferente = @IdOferente
                              ORDER BY op.fecha_postulacion DESC,
                                       op.id_oferente_puesto DESC
                              LIMIT 1;",
                            new { IdOferente = idOferente });

                    if (postulacion != null)
                    {
                        detalle.Puesto = new PuestoPostulacionDetalle
                        {
                            CodigoPuesto = postulacion.CodigoPuesto,
                            NombrePuesto = postulacion.NombrePuesto
                        };

                        if (!string.IsNullOrWhiteSpace(
                            postulacion.NombreArchivo))
                        {
                            detalle.Curriculum = new CurriculumDetalle
                            {
                                NombreArchivo = postulacion.NombreArchivo,
                                Mime = postulacion.Mime,
                                Tamanio = postulacion.Tamanio
                            };
                        }
                    }

                    return detalle;
                }
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(
                    "No fue posible consultar el detalle del oferente.",
                    ex);
            }
            catch (DataException ex)
            {
                throw new InvalidOperationException(
                    "No fue posible mapear el detalle del oferente.",
                    ex);
            }
        }

        private sealed class PostulacionAut3Row
        {
            public string CodigoPuesto { get; set; } = string.Empty;

            public string NombrePuesto { get; set; } = string.Empty;

            public string NombreArchivo { get; set; } = string.Empty;

            public string Mime { get; set; } = string.Empty;

            public int Tamanio { get; set; }
        }
    }
}
