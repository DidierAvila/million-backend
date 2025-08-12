using Million.Application.Authentications;
using Million.Application.Owners;
using Million.Application.Owners.Commands;
using Million.Application.Owners.Queries;
using Million.Application.Properties;
using Million.Application.Properties.Commands;
using Million.Application.Properties.Queries;
using Million.Application.PropertyImages;
using Million.Application.PropertyImages.Commands;
using Million.Application.PropertyImages.Queries;
using Million.Application.PropertyTraces;
using Million.Application.PropertyTraces.Commands;
using Million.Application.PropertyTraces.Queries;
using Million.Domain.Repositories;
using Million.Infrastructure.DbContexts;
using Million.Infrastructure.Repositories;

namespace Million.API.Extensions
{
    public static class ApiExtension
    {
        public static IServiceCollection AddApiExtention(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IPropertyRepository>(sp =>
            {
                var context = sp.GetRequiredService<IMillionDbContext>();
                return new PropertyRepository(context);
            });

            services.AddScoped<ITokenRepository>(sp =>
            {
                var context = sp.GetRequiredService<IMillionDbContext>();
                return new TokenRepository(context, "Tokens");
            });

            services.AddScoped<IOwnerRepository>(sp =>
            {
                var context = sp.GetRequiredService<IMillionDbContext>();
                return new OwnerRepository(context);
            });

            services.AddScoped<IUserRepository>(sp =>
            {
                var context = sp.GetRequiredService<IMillionDbContext>();
                return new UserRepository(context);
            });

            services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
            services.AddScoped<IPropertyTraceRepository, PropertyTraceRepository>();

            // Services
            services.AddScoped<IAuthentication, Authentication>();

            // CQRS Handlers
            services.AddScoped<IPropertyCommandHandler, PropertyCommandHandler>();
            services.AddScoped<IPropertyQueryHandler, PropertyQueryHandler>();
            services.AddScoped<PropertyFacade>();

            services.AddScoped<IOwnerCommandHandler, OwnerCommandHandler>();
            services.AddScoped<IOwnerQueryHandler, OwnerQueryHandler>();
            services.AddScoped<OwnerFacade>();

            services.AddScoped<IPropertyImageCommandHandler, PropertyImageCommandHandler>();
            services.AddScoped<IPropertyImageQueryHandler, PropertyImageQueryHandler>();
            services.AddScoped<PropertyImageFacade>();

            services.AddScoped<IPropertyTraceCommandHandler, PropertyTraceCommandHandler>();
            services.AddScoped<IPropertyTraceQueryHandler, PropertyTraceQueryHandler>();
            services.AddScoped<PropertyTraceFacade>();

            return services;
        }
    }
}
