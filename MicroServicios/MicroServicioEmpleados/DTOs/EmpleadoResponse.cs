namespace MicroServiciosEmpleados.DTOs;

public class EmpleadoResponse
{
    public int IdEmpleado { get; set; }
    public string NumeroEmpleado { get; set; } = string.Empty;
    public int IdOferente { get; set; }
    public int IdPuesto { get; set; }
    public DateTime FechaContratacion { get; set; }
    public string Estado { get; set; } = string.Empty;
}
