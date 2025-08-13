using Microsoft.Extensions.Logging;
using Million.Infrastructure.Settings;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Million.Infrastructure.Migrations.Examples
{
    public class UpdateOwnerPhotosToStrings : IMigration
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<UpdateOwnerPhotosToStrings> _logger;

        public int Version => 1;
        public string Name => "Update_Owner_Photos_To_Strings";
        public string Description => "Convierte el campo photo de byte[] a string para almacenar URLs de S3";

        public UpdateOwnerPhotosToStrings(
            IOptions<MongoDbSettings> options,
            ILogger<UpdateOwnerPhotosToStrings> logger)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            _logger = logger;
        }

        public async Task<bool> UpAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = _database.GetCollection<dynamic>("owners");
                
                // Definir la actualización
                var filter = Builders<dynamic>.Filter.Exists("photo");
                var update = Builders<dynamic>.Update.Rename("photo", "Photo").SetOnInsert("Photo", "");
                
                // Aplicar actualización
                var updateResult = await collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
                
                _logger.LogInformation($"Se actualizaron {updateResult.ModifiedCount} documentos de propietarios");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar los campos de fotos de propietarios");
                return false;
            }
        }

        public async Task<bool> DownAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var collection = _database.GetCollection<dynamic>("owners");
                
                // Definir la reversión
                var filter = Builders<dynamic>.Filter.Exists("Photo");
                var update = Builders<dynamic>.Update.Rename("Photo", "photo");
                
                // Aplicar actualización
                var updateResult = await collection.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
                
                _logger.LogInformation($"Se revirtieron {updateResult.ModifiedCount} documentos de propietarios");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al revertir los campos de fotos de propietarios");
                return false;
            }
        }
    }
}
