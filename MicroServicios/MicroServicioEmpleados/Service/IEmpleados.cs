using MicroServiciosEmpleados.Entities;

namespace MicroServiciosEmpleados.Services
{
    public interface IEmpleados
    {
        Task<string> RegistrarEmpleado(EntradaRegistrarEmpleado solicitud);
    }
}
