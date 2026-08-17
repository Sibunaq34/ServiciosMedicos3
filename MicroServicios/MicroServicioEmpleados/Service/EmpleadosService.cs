using MicroServicioEmpleados.Entities;
using MicroServiciosEmpleados.Repository;

namespace MicroServiciosEmpleados.Services;

public class EmpleadosService : IEmpleados
{
    private readonly EmpleadosRepository _empleadosRepository;

    public EmpleadosService(EmpleadosRepository empleadosRepository)
    {
        _empleadosRepository = empleadosRepository;
    }

    public async Task<CrearEmpleadoRepositoryResult> CrearEmpleado(CrearEmpleadoRequest request)
    {
        return await _empleadosRepository.CrearEmpleado(request);
    }
}
