namespace Million.Domain.DTOs
{
    public class OwnerDto
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public byte[]? photo { get; set; }
        public DateTime BirthDate { get; set; }
        public int PropertiesCount { get; set; }
        public string? Messages { get; set; }
        public bool Success { get; set; } = true;
    }

    public class CreateOwnerDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public byte[]? photo { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class UpdateOwnerDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public byte[]? photo { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
