using Microsoft.Extensions.Options;
using Million.Domain.Entities;
using Million.Infrastructure.Settings;
using MongoDB.Driver;

namespace Million.Infrastructure.DbContexts
{
    public interface IMillionDbContext
    {
        IMongoCollection<Property> Properties { get; }
        IMongoCollection<PropertyImage> PropertyImages { get; }
        IMongoCollection<Owner> Owners { get; }
        IMongoCollection<PropertyTrace> PropertyTraces { get; }
        IMongoCollection<User> Users { get; }
        IMongoCollection<Token> Tokens { get; }
    }

    public class MillionDbContext : IMillionDbContext
    {
        private readonly IMongoDatabase _database;

        public MillionDbContext(IOptions<MongoDbSettings> options)
        {
            var settings = options.Value;
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<Property> Properties => 
            _database.GetCollection<Property>("properties");
            
        public IMongoCollection<PropertyImage> PropertyImages => 
            _database.GetCollection<PropertyImage>("properties_image");
            
        public IMongoCollection<Owner> Owners => 
            _database.GetCollection<Owner>("owners");
            
        public IMongoCollection<PropertyTrace> PropertyTraces => 
            _database.GetCollection<PropertyTrace>("properties_trace");
            
        public IMongoCollection<User> Users => 
            _database.GetCollection<User>("users");
            
        public IMongoCollection<Token> Tokens => 
            _database.GetCollection<Token>("tokens");
    }
}
