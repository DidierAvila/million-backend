using Million.Domain.Entities;
using Million.Domain.Repositories;
using Million.Infrastructure.DbContexts;
using MongoDB.Driver;

namespace Million.Infrastructure.Repositories
{
    public class PropertyTraceRepository : BaseRepository<PropertyTrace>, IPropertyTraceRepository
    {
        public PropertyTraceRepository(IMillionDbContext context) 
            : base(context, "PropertyTraces")
        {
        }

        public async Task<IEnumerable<PropertyTrace>> GetTracesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<PropertyTrace>.Filter.Eq(x => x.IdProperty, propertyId);
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<PropertyTrace>> GetTracesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var filter = Builders<PropertyTrace>.Filter.And(
                Builders<PropertyTrace>.Filter.Gte(x => x.Date, startDate),
                Builders<PropertyTrace>.Filter.Lte(x => x.Date, endDate)
            );
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }
    }
}
