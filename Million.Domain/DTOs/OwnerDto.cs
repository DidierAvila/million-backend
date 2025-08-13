using Microsoft.AspNetCore.Http;

namespace Million.Domain.DTOs
{
    public class OwnerDto
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Photo { get; set; } // URL to the photo in S3
        public DateTime BirthDate { get; set; }
        public int PropertiesCount { get; set; }
        public string? Messages { get; set; }
        public bool Success { get; set; } = true;
    }

    public class CreateOwnerDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Photo { get; set; } // URL to the photo
        public IFormFile? PhotoFile { get; set; } // File upload
        public DateTime BirthDate { get; set; }
    }

    public class UpdateOwnerDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Photo { get; set; } // URL to the photo
        public IFormFile? PhotoFile { get; set; } // File upload
        public DateTime BirthDate { get; set; }
    }
}
