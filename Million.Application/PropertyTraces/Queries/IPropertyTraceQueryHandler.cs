using Million.Domain.DTOs;

namespace Million.Application.PropertyTraces.Queries
{
    public interface IPropertyTraceQueryHandler
    {
        Task<IEnumerable<PropertyTraceDto>> GetAllPropertyTracesAsync(CancellationToken cancellationToken);
        Task<PropertyTraceDto> GetPropertyTraceByIdAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<PropertyTraceDto>> GetTracesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken);
        Task<IEnumerable<PropertyTraceDto>> GetTracesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    }
}
