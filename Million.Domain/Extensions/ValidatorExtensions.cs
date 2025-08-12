using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Million.Domain.DTOs;
using Million.Domain.Validators;
using System.Reflection;

namespace Million.Domain.Extensions
{
    public static class ValidatorExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            // Registrar validadores para Owner
            services.AddScoped<IValidator<CreateOwnerDto>, CreateOwnerDtoValidator>();
            services.AddScoped<IValidator<UpdateOwnerDto>, UpdateOwnerDtoValidator>();

            // Registrar validadores para Property
            services.AddScoped<IValidator<CreatePropertyDto>, CreatePropertyDtoValidator>();
            services.AddScoped<IValidator<UpdatePropertyDto>, UpdatePropertyDtoValidator>();

            // Registrar validadores para PropertyTrace
            services.AddScoped<IValidator<CreatePropertyTraceDto>, CreatePropertyTraceDtoValidator>();

            // Registrar validadores para PropertyImage
            services.AddScoped<IValidator<CreatePropertyImageDto>, CreatePropertyImageDtoValidator>();
            services.AddScoped<IValidator<UpdatePropertyImageDto>, UpdatePropertyImageDtoValidator>();

            return services;
        }
    }
}
