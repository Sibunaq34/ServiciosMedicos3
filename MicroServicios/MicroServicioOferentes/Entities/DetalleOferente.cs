using System;
using System.Collections.Generic;

namespace MicroServicioOferentes.Entities
{   
    // Persona C - Kenneth
    // Agrupa el detalle registrado por AUT3 para CORE8.
    public class DetalleOferente
    {
        public int IdOferente { get; set; }

        public string Identificacion { get; set; } = string.Empty;

        public string TipoIdentificacion { get; set; } = string.Empty;

        public string NombreCompleto { get; set; } = string.Empty;

        public DateTime FechaNacimiento { get; set; }

        public List<string> Correos { get; set; } = new List<string>();

        public List<string> Telefonos { get; set; } = new List<string>();

        public PuestoPostulacionDetalle Puesto { get; set; }

        public CurriculumDetalle Curriculum { get; set; }
    }
}
