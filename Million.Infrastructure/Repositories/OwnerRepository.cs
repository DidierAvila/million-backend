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

        public Task<Owner?> GetOwnerByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Owner>> GetOwnersByBirthDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Owner>> GetOwnersByNameContainingAsync(string name, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Owner>.Filter.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }
    }
}
