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
    }
}
