namespace Million.Domain.Entities
{
    public class Propietario
    {
        public int IdOwner { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }

        // Navigation properties
        public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
    }
}