using Million.Domain.Entities;
using Million.Domain.Repositories;
using Million.Infrastructure.DbContexts;
using MongoDB.Driver;

namespace Million.Infrastructure.Repositories
{
    public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(IMillionDbContext context) : base(context, "Properties")
        {
        }

        public async Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(string ownerId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Property>.Filter.Eq(p => p.IdOwner, ownerId);
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Property>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Property>.Filter.And(
                Builders<Property>.Filter.Gte(p => p.Price, minPrice),
                Builders<Property>.Filter.Lte(p => p.Price, maxPrice)
            );
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Property>> GetPropertiesByYearAsync(int year, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Property>.Filter.Eq(p => p.Year, year);
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Property>> GetPropertiesWithFiltersAsync(
            string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice,
            CancellationToken cancellationToken = default)
        {
            var filters = new List<FilterDefinition<Property>>();

            if (!string.IsNullOrWhiteSpace(name))
                filters.Add(Builders<Property>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i")));

            if (!string.IsNullOrWhiteSpace(address))
                filters.Add(Builders<Property>.Filter.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(address, "i")));

            if (minPrice.HasValue)
                filters.Add(Builders<Property>.Filter.Gte(p => p.Price, minPrice.Value));

            if (maxPrice.HasValue)
                filters.Add(Builders<Property>.Filter.Lte(p => p.Price, maxPrice.Value));

            var filter = filters.Any()
                ? Builders<Property>.Filter.And(filters)
                : Builders<Property>.Filter.Empty;

            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }
    }
}
