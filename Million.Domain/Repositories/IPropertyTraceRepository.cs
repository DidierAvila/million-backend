using Million.Domain.Entities;

namespace Million.Domain.Repositories
{
    public interface IPropertyTraceRepository : IRepository<PropertyTrace>
    {
        Task<IEnumerable<PropertyTrace>> GetTracesByPropertyIdAsync(string propertyId);
        Task<IEnumerable<PropertyTrace>> GetTracesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
