namespace Million.Application.DTOs
{
    public class PropiedadCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal Impuestos { get; set; }
        public int Año { get; set; }
        public string CodigoInterno { get; set; } = string.Empty;
        public int IdOwner { get; set; }
    }

    public class PropiedadUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public decimal Impuestos { get; set; }
        public int Año { get; set; }
        public string CodigoInterno { get; set; } = string.Empty;
        public int IdOwner { get; set; }
    }
}