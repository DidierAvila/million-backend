using Million.Application.Authentications;
using Million.Application.Owners;
using Million.Application.Owners.Commands;
using Million.Application.Owners.Queries;
using Million.Application.Properties;
using Million.Application.Properties.Commands;
using Million.Application.Properties.Queries;
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

            // Services
            services.AddScoped<IAuthentication, Authentication>();

            // CQRS Handlers
            services.AddScoped<IPropertyCommandHandler, PropertyCommandHandler>();
            services.AddScoped<IPropertyQueryHandler, PropertyQueryHandler>();
            services.AddScoped<PropertyFacade>();

            services.AddScoped<IOwnerCommandHandler, OwnerCommandHandler>();
            services.AddScoped<IOwnerQueryHandler, OwnerQueryHandler>();
            services.AddScoped<OwnerFacade>();

            return services;
        }
    }
}
