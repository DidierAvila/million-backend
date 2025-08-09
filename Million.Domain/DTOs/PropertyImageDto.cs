namespace Million.Domain.DTOs
{
    public class PropertyImageDto
    {
        public string? Id { get; set; }
        public string PropertyId { get; set; } = null!;
        public byte[] File { get; set; } = null!;
        public bool Enabled { get; set; }
        public string? PropertyName { get; set; } // Información adicional de la propiedad
    }

    public class CreatePropertyImageDto
    {
        public string PropertyId { get; set; } = null!;
        public byte[] File { get; set; } = null!;
        public bool Enabled { get; set; } = true;
    }

    public class UpdatePropertyImageDto
    {
        public bool Enabled { get; set; }
    }
}
