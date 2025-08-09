namespace Million.Domain.Entities
{
    public class ImagenPropiedad
    {
        public int IdPropertyImage { get; set; }
        public int IdProperty { get; set; }
        public string Archivo { get; set; } = string.Empty;
        public bool Habilitado { get; set; }

        // Navigation property
        public Propiedad Propiedad { get; set; } = null!;
    }
}