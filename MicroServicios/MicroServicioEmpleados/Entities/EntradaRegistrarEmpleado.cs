using System.Runtime.Serialization;

namespace MicroServiciosEmpleados.Entities
{
    [DataContract]
    public class EntradaRegistrarEmpleado
    {
        [DataMember(IsRequired = true, Order = 1)]
        public int IdOferente { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public string CodigoPuesto { get; set; } = string.Empty;

        [DataMember(EmitDefaultValue = false, Order = 3)]
        public int? IdJefatura { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public int IdUsuario { get; set; }
    }
}