namespace Million.Domain.DTOs
{
    public class PropertyTraceDto
    {
        public string IdPropertyTrace { get; set; } = null!;
        public string PropertyId { get; set; } = null!;
        public DateTime SaleDate { get; set; }
        public decimal Value { get; set; }
        public string? PropertyName { get; set; }
        public string? Messages { get; set; }
        public bool Success { get; set; } = true;
    }

    public class CreatePropertyTraceDto
    {
        public string PropertyId { get; set; } = null!;
        public DateTime SaleDate { get; set; }
        public decimal Value { get; set; }
    }

    public class UpdatePropertyTraceDto
    {
        public DateTime SaleDate { get; set; }
        public decimal Value { get; set; }
    }
}
