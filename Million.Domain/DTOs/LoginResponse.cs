namespace Million.Domain.DTOs
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public string? Messages { get; set; }
        public bool Success { get; set; } = true;
    }
}
