using Million.Domain.Entities;

namespace Million.Domain.Repositories
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        Task<IEnumerable<Owner>> GetOwnersByBirthDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Owner?> GetOwnerByNameAsync(string name);
    }
}
