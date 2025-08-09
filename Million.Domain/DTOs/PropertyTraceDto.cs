namespace Million.Domain.DTOs
{
    public class PropertyTraceDto
    {
        public string? Id { get; set; }
        public string PropertyId { get; set; } = null!;
        public DateTime SaleDate { get; set; }
        public decimal Value { get; set; }
        public string? PropertyName { get; set; } // Información adicional de la propiedad
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
