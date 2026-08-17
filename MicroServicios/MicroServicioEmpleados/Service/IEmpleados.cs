using MicroServicioEmpleados.Entities;
using MicroServiciosEmpleados.Repository;

namespace MicroServiciosEmpleados.Services;

public interface IEmpleados
{
    Task<CrearEmpleadoRepositoryResult> CrearEmpleado(CrearEmpleadoRequest request);
}
