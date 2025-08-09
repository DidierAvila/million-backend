using Million.Domain.Entities;
using Million.Domain.Repositories;
using Million.Infrastructure.DbContexts;

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
    }
}
