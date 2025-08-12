using Million.Domain.DTOs;

namespace Million.Application.Properties.Queries
{
    public interface IPropertyQueryHandler
    {
        Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync(CancellationToken cancellationToken);
        Task<PropertyDto> GetPropertyByIdAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<PropertyDto>> GetPropertiesWithFiltersAsync(
            string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice,
            CancellationToken cancellationToken);
    }
}
