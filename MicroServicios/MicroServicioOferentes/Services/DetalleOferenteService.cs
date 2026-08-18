using MicroServicioOferentes.Repository;
using MicroServicioOferentes.Entities;

namespace MicroServicioOferentes.Services
{
    // Persona C - Kenneth
    // Coordina la consulta y construye la respuesta de CORE8.
    public class DetalleOferenteService : IDetalleOferente
    {
        private readonly DetalleOferenteRepository _repository;

        public DetalleOferenteService(DetalleOferenteRepository repository) { 
            _repository = repository;
        }

        public async Task<ResultadoDetalleOferente> ObtenerDetalleAsync(
            int idOferente)
        {
            if (idOferente <= 0)
            {
                return CrearRespuesta(
                    false,
                    "El id del oferente debe ser mayor que cero.",
                    null);
            }

            try
            {
                var detalle =
                    await _repository.ObtenerDetalleAsync(idOferente);

                if (detalle == null)
                {
                    return CrearRespuesta(
                        false,
                        "El oferente indicado no existe.",
                        null);
                }

                return CrearRespuesta(
                    true,
                    "Consulta realizada correctamente.",
                    detalle);
            }
            catch (InvalidOperationException)
            {
                return CrearRespuesta(
                    false,
                    "No fue posible consultar el detalle del oferente.",
                    null);
            }
            catch (Exception)
            {
                return CrearRespuesta(
                    false,
                    "Ocurrió un error no previsto al consultar el oferente.",
                    null);
            }
        }

        private static ResultadoDetalleOferente CrearRespuesta(
            bool exito,
            string mensaje,
            DetalleOferente datos)
        {
            return new ResultadoDetalleOferente
            {
                Exito = exito,
                Mensaje = mensaje,
                Datos = datos
            };
        }
    }
}
