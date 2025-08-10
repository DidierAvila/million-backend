using Million.Domain.Entities;

namespace Million.Domain.Repositories
{
    public interface IPropertyTraceRepository : IRepository<PropertyTrace>
    {
        Task<IEnumerable<PropertyTrace>> GetTracesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PropertyTrace>> GetTracesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
