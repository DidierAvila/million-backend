namespace Million.Domain.Entities
{
    public class TrazabilidadPropiedad
    {
        public int IdPropertyTrace { get; set; }
        public int IdProperty { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal Valor { get; set; }

        // Navigation property
        public Propiedad Propiedad { get; set; } = null!;
    }
}