using Million.Domain.Entities;
using Million.Domain.Repositories;
using Million.Infrastructure.DbContexts;
using MongoDB.Driver;

namespace Million.Infrastructure.Repositories
{
    public class PropertyImageRepository : BaseRepository<PropertyImage>, IPropertyImageRepository
    {
        public PropertyImageRepository(IMillionDbContext context) 
            : base(context, "PropertyImages")
        {
        }

        public async Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken)
        {
            var filter = Builders<PropertyImage>.Filter.Eq(x => x.IdProperty, propertyId);
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PropertyImage>> GetEnabledImagesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken)
        {
            var filter = Builders<PropertyImage>.Filter.And(
                Builders<PropertyImage>.Filter.Eq(x => x.IdProperty, propertyId),
                Builders<PropertyImage>.Filter.Eq(x => x.Enabled, true)
            );
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }
    }
}
