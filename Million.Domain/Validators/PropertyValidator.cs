using FluentValidation;
using Million.Domain.DTOs;
using System;
using System.Text.RegularExpressions;

namespace Million.Domain.Validators
{
    public class CreatePropertyDtoValidator : AbstractValidator<CreatePropertyDto>
    {
        public CreatePropertyDtoValidator()
        {
            RuleFor(property => property.Name)
                .NotEmpty().WithMessage("El nombre de la propiedad es requerido")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres");

            RuleFor(property => property.Address)
                .NotEmpty().WithMessage("La dirección de la propiedad es requerida")
                .Length(5, 200).WithMessage("La dirección debe tener entre 5 y 200 caracteres");

            RuleFor(property => property.Price)
                .NotEmpty().WithMessage("El precio de la propiedad es requerido")
                .GreaterThan(0).WithMessage("El precio debe ser mayor que cero");

            RuleFor(property => property.Year)
                .NotEmpty().WithMessage("El año de construcción es requerido")
                .InclusiveBetween(1800, DateTime.Now.Year).WithMessage($"El año debe estar entre 1800 y {DateTime.Now.Year}");

            RuleFor(property => property.InternalCode)
                .NotEmpty().WithMessage("El código interno de la propiedad es requerido")
                .Length(3, 20).WithMessage("El código interno debe tener entre 3 y 20 caracteres")
                .Matches(new Regex(@"^[a-zA-Z0-9\-]+$")).WithMessage("El código interno solo puede contener letras, números y guiones");

            RuleFor(property => property.IdOwner)
                .NotEmpty().WithMessage("El ID del propietario es requerido")
                .Matches(new Regex(@"^[a-f0-9]{24}$")).WithMessage("El ID del propietario debe ser un ObjectId válido de MongoDB");
        }
    }

    public class UpdatePropertyDtoValidator : AbstractValidator<UpdatePropertyDto>
    {
        public UpdatePropertyDtoValidator()
        {
            RuleFor(property => property.Name)
                .NotEmpty().WithMessage("El nombre de la propiedad es requerido")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres");

            RuleFor(property => property.Address)
                .NotEmpty().WithMessage("La dirección de la propiedad es requerida")
                .Length(5, 200).WithMessage("La dirección debe tener entre 5 y 200 caracteres");

            RuleFor(property => property.Price)
                .NotEmpty().WithMessage("El precio de la propiedad es requerido")
                .GreaterThan(0).WithMessage("El precio debe ser mayor que cero");

            RuleFor(property => property.Year)
                .NotEmpty().WithMessage("El año de construcción es requerido")
                .InclusiveBetween(1800, DateTime.Now.Year).WithMessage($"El año debe estar entre 1800 y {DateTime.Now.Year}");

            RuleFor(property => property.InternalCode)
                .NotEmpty().WithMessage("El código interno de la propiedad es requerido")
                .Length(3, 20).WithMessage("El código interno debe tener entre 3 y 20 caracteres")
                .Matches(new Regex(@"^[a-zA-Z0-9\-]+$")).WithMessage("El código interno solo puede contener letras, números y guiones");

            RuleFor(property => property.IdOwner)
                .NotEmpty().WithMessage("El ID del propietario es requerido")
                .Matches(new Regex(@"^[a-f0-9]{24}$")).WithMessage("El ID del propietario debe ser un ObjectId válido de MongoDB");
        }
    }
}
