namespace Million.Domain.DTOs
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string NotificationType { get; set; } = null!;
    }

    public class CreateUserDto
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string NotificationType { get; set; } = null!;
    }

    public class UpdateUserDto
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string NotificationType { get; set; } = null!;
    }

    public class LoginUserDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
