using System.ComponentModel.DataAnnotations;

namespace MicroServicioEmpleados.Entities;

public class CrearEmpleadoRequest
{
    [Range(1, int.MaxValue)]
    public int IdOferente { get; set; }

    [Range(1, int.MaxValue)]
    public int IdPuesto { get; set; }

    [Range(1, int.MaxValue)]
    public int IdJefatura { get; set; }
}
