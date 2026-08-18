using MicroServiciosEmpleados.Repository;
using MicroServiciosEmpleados.Entities;

namespace MicroServiciosEmpleados.Services
{
    public class EmpleadosService : IEmpleados
    {
        private readonly EmpleadosRepository _empleadosBD;

        public EmpleadosService(
            EmpleadosRepository empleadosBD)
        {
            _empleadosBD = empleadosBD;
        }

        public async Task<string> RegistrarEmpleado(EntradaRegistrarEmpleado solicitud)
        {
            solicitud.CodigoPuesto = solicitud.CodigoPuesto.Trim();

            var validacion = await _empleadosBD.ValidarContratacion(solicitud);

            if (!string.IsNullOrEmpty(validacion))
                return validacion;

            var registrado = await _empleadosBD.RegistrarEmpleado(solicitud);
            return registrado ? string.Empty : "REGISTRATION_FAILED";
        }

    }
}
