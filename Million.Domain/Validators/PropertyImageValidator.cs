using FluentValidation;
using Million.Domain.DTOs;
using System.Text.RegularExpressions;

namespace Million.Domain.Validators
{
    public class CreatePropertyImageDtoValidator : AbstractValidator<CreatePropertyImageDto>
    {
        public CreatePropertyImageDtoValidator()
        {
            RuleFor(image => image.IdProperty)
                .NotEmpty().WithMessage("El ID de la propiedad es requerido")
                .Matches(new Regex(@"^[a-f0-9]{24}$")).WithMessage("El ID de la propiedad debe ser un ObjectId válido de MongoDB");

            RuleFor(image => image.File)
                .NotEmpty().WithMessage("La URL de la imagen es requerida")
                .Must(BeValidUrl).WithMessage("La URL de la imagen no es válida");
        }

        private bool BeValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            // Verifica si es una URL de S3 o una URL web válida
            Uri uriResult;
            var isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            
            var isS3Url = url.Contains("amazonaws.com") || url.Contains("s3.") || url.StartsWith("s3://");

            return isValidUrl || isS3Url;
        }
    }

    public class UpdatePropertyImageDtoValidator : AbstractValidator<UpdatePropertyImageDto>
    {
        public UpdatePropertyImageDtoValidator()
        {
            RuleFor(image => image.File)
                .Must(BeValidUrl).When(x => !string.IsNullOrEmpty(x.File))
                .WithMessage("La URL de la imagen no es válida");
        }

        private bool BeValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return true; // En actualización, puede ser nulo

            // Verifica si es una URL de S3 o una URL web válida
            Uri uriResult;
            var isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult) 
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            
            var isS3Url = url.Contains("amazonaws.com") || url.Contains("s3.") || url.StartsWith("s3://");

            return isValidUrl || isS3Url;
        }
    }
}
