namespace Million.Domain.Entities
{
    public class Propiedad
    {
        public int IdProperty { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal Impuestos { get; set; }
        public int Año { get; set; }
        public string CodigoInterno { get; set; } = string.Empty;
        public int IdOwner { get; set; }

        // Navigation properties
        public Propietario Propietario { get; set; } = null!;
        public ICollection<ImagenPropiedad> ImagenesPropiedad { get; set; } = new List<ImagenPropiedad>();
        public ICollection<TrazabilidadPropiedad> TrazabilidadesPropiedad { get; set; } = new List<TrazabilidadPropiedad>();
    }
}