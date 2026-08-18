namespace MicroServicioOferentes.Entities
{
    // Persona C - Kenneth
    // Expone solo metadatos seguros del curriculum registrado en AUT3.
    public class CurriculumDetalle
    {
        public string NombreArchivo { get; set; } = string.Empty;

        public string Mime { get; set; } = string.Empty;

        public int Tamanio { get; set; }
    }
}
