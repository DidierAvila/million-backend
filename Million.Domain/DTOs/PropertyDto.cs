namespace Million.Domain.DTOs
{
    public class PropertyDto
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Taxes { get; set; }
        public int Year { get; set; }
        public string InternalCode { get; set; } = null!;
        public string IdOwner { get; set; } = null!;
        public string? OwnerName { get; set; }
        public string? Messages { get; set; }
        public bool Success { get; set; } = true;
    }

    public class CreatePropertyDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Taxes { get; set; }
        public int Year { get; set; }
        public string InternalCode { get; set; } = null!;
        public string IdOwner { get; set; } = null!;
        public ICollection<CreatePropertyImageDto>? Images { get; set; }
    }

    public class UpdatePropertyDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Taxes { get; set; }
        public int Year { get; set; }
        public string InternalCode { get; set; } = null!;
        public string IdOwner { get; set; } = null!;
    }
}
