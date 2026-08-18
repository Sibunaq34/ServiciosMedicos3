
namespace MicroServicioPuestos.Entities
{
    public class Puestos
    {
        public int IdPuesto { get; set; }

        public string CodigoPuesto { get; set; } = string.Empty;

        public string NombrePuesto { get; set; } = string.Empty;

        public decimal MontoSalario { get; set; }

        public int? IdPuestoJefac { get; set; }

        public string? Jefatura { get; set; }

        public int? pagina { get; set; }
    }
}