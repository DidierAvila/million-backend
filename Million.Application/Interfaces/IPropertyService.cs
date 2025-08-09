using Million.Domain.DTOs;

namespace Million.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync();
        Task<PropertyDto?> GetPropertyByIdAsync(string id);
        Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createPropertyDto);
        Task<bool> UpdatePropertyAsync(string id, UpdatePropertyDto updatePropertyDto);
        Task<bool> DeletePropertyAsync(string id);
        Task<IEnumerable<PropertyDto>> GetPropertiesByOwnerAsync(string ownerId);
        Task<IEnumerable<PropertyDto>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<PropertyDto>> GetPropertiesByYearAsync(int year);
    }
}
