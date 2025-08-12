using Million.Domain.Entities;
using Million.Domain.Repositories;
using Million.Infrastructure.DbContexts;
using MongoDB.Driver;

namespace Million.Infrastructure.Repositories
{
    public class OwnerRepository : BaseRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(IMillionDbContext context) 
            : base(context, "Owners")
        {
        }

        public async Task<Owner?> GetOwnerByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _collection.Find(x => x.Name == name).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Owner>> GetOwnersByBirthDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            return await _collection
                    .Find(x => x.BirthDate >= startDate && x.BirthDate <= endDate)
                    .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Owner>> GetOwnersByNameContainingAsync(string name, CancellationToken cancellationToken)
        {
            var filter = Builders<Owner>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }
    }
}
