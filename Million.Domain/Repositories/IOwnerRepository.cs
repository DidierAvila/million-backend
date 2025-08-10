using Million.Domain.Entities;

namespace Million.Domain.Repositories
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        Task<IEnumerable<Owner>> GetOwnersByBirthDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<Owner?> GetOwnerByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Owner>> GetOwnersByNameContainingAsync(string name, CancellationToken cancellationToken = default);
    }
}
