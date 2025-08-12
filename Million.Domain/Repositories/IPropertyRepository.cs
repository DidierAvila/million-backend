using Million.Domain.Entities;

namespace Million.Domain.Repositories
{
    public interface IPropertyRepository : IRepository<Property>
    {
        Task<IEnumerable<Property>> GetPropertiesByOwnerAsync(string ownerId, CancellationToken cancellationToken);
        Task<IEnumerable<Property>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken);
        Task<IEnumerable<Property>> GetPropertiesByYearAsync(int year, CancellationToken cancellationToken);
        Task<IEnumerable<Property>> GetPropertiesWithFiltersAsync(
            string? name,
            string? address,
            decimal? minPrice,
            decimal? maxPrice,
            CancellationToken cancellationToken);
    }
}
