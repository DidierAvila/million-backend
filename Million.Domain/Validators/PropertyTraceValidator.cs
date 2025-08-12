using FluentValidation;
using Million.Domain.DTOs;
using System;
using System.Text.RegularExpressions;

namespace Million.Domain.Validators
{
    public class CreatePropertyTraceDtoValidator : AbstractValidator<CreatePropertyTraceDto>
    {
        public CreatePropertyTraceDtoValidator()
        {
            RuleFor(trace => trace.PropertyId)
                .NotEmpty().WithMessage("El ID de la propiedad es requerido")
                .Matches(new Regex(@"^[a-f0-9]{24}$")).WithMessage("El ID de la propiedad debe ser un ObjectId válido de MongoDB");

            RuleFor(trace => trace.Date)
                .NotEmpty().WithMessage("La fecha de la traza es requerida")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha debe ser igual o anterior a la fecha actual");

            RuleFor(trace => trace.Value)
                .NotEmpty().WithMessage("El valor de la operación es requerido")
                .GreaterThanOrEqualTo(0).WithMessage("El valor debe ser mayor o igual a cero");

            RuleFor(trace => trace.Tax)
                .NotEmpty().WithMessage("El impuesto es requerido")
                .GreaterThanOrEqualTo(0).WithMessage("El impuesto debe ser mayor o igual a cero");

            RuleFor(trace => trace.Name)
                .NotEmpty().WithMessage("El nombre de la operación es requerido")
                .Length(2, 100).WithMessage("El nombre debe tener entre 2 y 100 caracteres");

            RuleFor(trace => trace.Operation)
                .NotEmpty().WithMessage("El tipo de operación es requerido")
                .Must(BeValidOperation).WithMessage("La operación debe ser CREATE, UPDATE o DELETE");
        }

        private bool BeValidOperation(string operation)
        {
            var validOperations = new[] { "CREATE", "UPDATE", "DELETE" };
            return validOperations.Contains(operation.ToUpper());
        }
    }
}
