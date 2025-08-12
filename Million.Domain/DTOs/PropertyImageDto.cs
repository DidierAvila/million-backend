namespace Million.Domain.DTOs
{
    public class PropertyImageDto
    {
        public string IdPropertyImage { get; set; } = null!;
        public string IdProperty { get; set; } = null!;
        public string File { get; set; } = null!;
        public bool Enabled { get; set; }
        public string? PropertyName { get; set; }
        public string? Messages { get; set; }
        public bool Success { get; set; } = true;
    }

    public class CreatePropertyImageDto
    {
        public string IdProperty { get; set; } = null!;
        public string File { get; set; } = null!;
        public bool Enabled { get; set; } = true;
    }

    public class UpdatePropertyImageDto
    {
        public string? File { get; set; }
        public bool? Enabled { get; set; }
    }
}
