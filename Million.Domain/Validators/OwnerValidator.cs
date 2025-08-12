using FluentValidation;
using Million.Domain.DTOs;
using System;

namespace Million.Domain.Validators
{
    public class CreateOwnerDtoValidator : AbstractValidator<CreateOwnerDto>
    {
        public CreateOwnerDtoValidator()
        {
            RuleFor(owner => owner.Name)
                .NotEmpty().WithMessage("El nombre del propietario es requerido")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres");

            RuleFor(owner => owner.Address)
                .NotEmpty().WithMessage("La dirección del propietario es requerida")
                .Length(5, 200).WithMessage("La dirección debe tener entre 5 y 200 caracteres");

            RuleFor(owner => owner.BirthDate)
                .NotEmpty().WithMessage("La fecha de nacimiento es requerida")
                .LessThan(DateTime.Now).WithMessage("La fecha de nacimiento debe ser anterior a la fecha actual")
                .Must(BeAValidAge).WithMessage("La edad debe ser mayor a 18 años");
        }

        private bool BeAValidAge(DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }
    }

    public class UpdateOwnerDtoValidator : AbstractValidator<UpdateOwnerDto>
    {
        public UpdateOwnerDtoValidator()
        {
            RuleFor(owner => owner.Name)
                .NotEmpty().WithMessage("El nombre del propietario es requerido")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres");

            RuleFor(owner => owner.Address)
                .NotEmpty().WithMessage("La dirección del propietario es requerida")
                .Length(5, 200).WithMessage("La dirección debe tener entre 5 y 200 caracteres");

            RuleFor(owner => owner.BirthDate)
                .NotEmpty().WithMessage("La fecha de nacimiento es requerida")
                .LessThan(DateTime.Now).WithMessage("La fecha de nacimiento debe ser anterior a la fecha actual")
                .Must(BeAValidAge).WithMessage("La edad debe ser mayor a 18 años");
        }

        private bool BeAValidAge(DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }
    }
}
