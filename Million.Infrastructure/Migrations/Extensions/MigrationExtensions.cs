using Microsoft.Extensions.DependencyInjection;
using Million.Infrastructure.Migrations.Examples;

namespace Million.Infrastructure.Migrations.Extensions
{
    public static class MigrationExtensions
    {
        public static IServiceCollection AddMigrations(this IServiceCollection services)
        {
            // Registrar el servicio de migración
            services.AddScoped<MigrationService>();
            
            // Registrar las migraciones individuales
            services.AddScoped<IMigration, UpdateOwnerPhotosToStrings>();
            
            // Puedes agregar más migraciones aquí
            
            return services;
        }
    }
}
