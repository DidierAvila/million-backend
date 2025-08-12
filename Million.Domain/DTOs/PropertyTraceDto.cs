namespace Million.Domain.DTOs
{
    public class PropertyTraceDto
    {
        public string IdPropertyTrace { get; set; } = null!;
        public string PropertyId { get; set; } = null!;
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public string Name { get; set; } = null!;
        public string Operation { get; set; } = null!;
        public string? PropertyName { get; set; }
        public string? Messages { get; set; }
        public bool Success { get; set; } = true;
    }

    public class CreatePropertyTraceDto
    {
        public string PropertyId { get; set; } = null!;
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
        public string Name { get; set; } = null!;
        public string Operation { get; set; } = null!;
    }
}
