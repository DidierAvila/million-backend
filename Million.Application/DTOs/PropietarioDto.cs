namespace Million.Application.DTOs
{
    public class PropietarioCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
    }

    public class PropietarioUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
    }
}