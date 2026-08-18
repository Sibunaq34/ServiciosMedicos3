namespace MicroServicioOferentes.Entities
{
    // Persona C - Kenneth
    // Envuelve la respuesta SOAP controlada para CORE8.
    public class ResultadoDetalleOferente
    {
        public bool Exito { get; set; }

        public string Mensaje { get; set; } = string.Empty;

        public DetalleOferente Datos { get; set; }
    }
}
