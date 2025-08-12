using Million.Domain.Entities;

namespace Million.Domain.Repositories
{
    public interface IPropertyRepository : IRepository<Property>
    {
        Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(string ownerId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Property>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
        Task<IEnumerable<Property>> GetPropertiesByYearAsync(int year, CancellationToken cancellationToken = default);
        Task<IEnumerable<Property>> GetPropertiesWithFiltersAsync(
            string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice,
            CancellationToken cancellationToken = default);
    }
}
