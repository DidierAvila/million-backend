namespace Million.Domain.DTOs
{
    public class TokenDto
    {
        public string? Id { get; set; }
        public string UserId { get; set; } = null!;
        public string? TokenValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Status { get; set; }
        public string? UserEmail { get; set; } // Información adicional del usuario
    }

    public class CreateTokenDto
    {
        public string UserId { get; set; } = null!;
        public string TokenValue { get; set; } = null!;
        public DateTime ExpirationDate { get; set; }
    }

    public class UpdateTokenDto
    {
        public bool Status { get; set; }
    }
}
