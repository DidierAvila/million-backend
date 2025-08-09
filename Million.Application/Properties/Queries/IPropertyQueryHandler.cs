using Million.Domain.DTOs;

namespace Million.Application.Properties.Queries
{
    public interface IPropertyQueryHandler
    {
        Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync();
        Task<PropertyDto?> GetPropertyByIdAsync(string id);
        Task<IEnumerable<PropertyDto>> GetPropertiesByOwnerAsync(string ownerId);
        Task<IEnumerable<PropertyDto>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<PropertyDto>> GetPropertiesByYearAsync(int year);
    }
}
