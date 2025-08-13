using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Million.Infrastructure.Settings;
using MongoDB.Driver;

namespace Million.Infrastructure.Migrations
{
    public class MigrationService
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<MigrationService> _logger;
        private readonly IEnumerable<IMigration> _migrations;
        private readonly IMongoCollection<Migration> _migrationCollection;

        public MigrationService(
            IOptions<MongoDbSettings> options, 
            ILogger<MigrationService> logger,
            IEnumerable<IMigration>? migrations = null)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            
            _logger = logger;
            _migrations = migrations?.OrderBy(m => m.Version).ToList() ?? new List<IMigration>();
            
            // Asegúrate de que la colección de migraciones exista
            _migrationCollection = _database.GetCollection<Migration>("migrations");
        }

        public async Task<bool> MigrateUpAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Iniciando proceso de migración hacia arriba");
                
                // Obtener todas las migraciones aplicadas
                var appliedMigrations = await _migrationCollection
                    .Find(m => m.Success)
                    .SortBy(m => m.Version)
                    .ToListAsync(cancellationToken);
                
                int lastVersion = appliedMigrations.Any() ? appliedMigrations.Max(m => m.Version) : 0;
                
                // Obtener migraciones pendientes
                var pendingMigrations = _migrations.Where(m => m.Version > lastVersion).OrderBy(m => m.Version);
                
                foreach (var migration in pendingMigrations)
                {
                    _logger.LogInformation($"Aplicando migración {migration.Version}: {migration.Name}");
                    
                    bool success = false;
                    try
                    {
                        success = await migration.UpAsync(cancellationToken);
                        
                        // Registrar la migración
                        await _migrationCollection.InsertOneAsync(
                            new Migration
                            {
                                Name = migration.Name,
                                Version = migration.Version,
                                Description = migration.Description,
                                Applied = DateTime.UtcNow,
                                Success = success
                            }, 
                            null, 
                            cancellationToken);
                        
                        _logger.LogInformation($"Migración {migration.Version}: {migration.Name} aplicada correctamente");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error al aplicar migración {migration.Version}: {migration.Name}");
                        return false;
                    }
                }
                
                _logger.LogInformation("Proceso de migración completado correctamente");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el proceso de migración");
                return false;
            }
        }

        public async Task<bool> MigrateDownToVersionAsync(int targetVersion, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"Iniciando proceso de migración hacia abajo hasta la versión {targetVersion}");
                
                // Obtener todas las migraciones aplicadas
                var appliedMigrations = await _migrationCollection
                    .Find(m => m.Success && m.Version > targetVersion)
                    .SortByDescending(m => m.Version)
                    .ToListAsync(cancellationToken);
                
                if (!appliedMigrations.Any())
                {
                    _logger.LogInformation("No hay migraciones para revertir");
                    return true;
                }
                
                // Revertir migraciones en orden inverso
                foreach (var appliedMigration in appliedMigrations)
                {
                    var migration = _migrations.FirstOrDefault(m => m.Version == appliedMigration.Version);
                    if (migration == null)
                    {
                        _logger.LogWarning($"No se encontró la implementación para la migración {appliedMigration.Version}: {appliedMigration.Name}");
                        continue;
                    }
                    
                    _logger.LogInformation($"Revirtiendo migración {migration.Version}: {migration.Name}");
                    
                    try
                    {
                        bool success = await migration.DownAsync(cancellationToken);
                        
                        if (success)
                        {
                            // Eliminar el registro de la migración
                            await _migrationCollection.DeleteOneAsync(
                                m => m.Version == appliedMigration.Version,
                                cancellationToken);
                            
                            _logger.LogInformation($"Migración {migration.Version}: {migration.Name} revertida correctamente");
                        }
                        else
                        {
                            _logger.LogError($"Error al revertir migración {migration.Version}: {migration.Name}");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error al revertir migración {migration.Version}: {migration.Name}");
                        return false;
                    }
                }
                
                _logger.LogInformation("Proceso de reversión de migraciones completado correctamente");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el proceso de reversión de migraciones");
                return false;
            }
        }
    }
}
