using Million.Domain.DTOs;

namespace Million.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync(CancellationToken cancellationToken = default);
        Task<PropertyDto?> GetPropertyByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createPropertyDto, CancellationToken cancellationToken = default);
        Task<bool> UpdatePropertyAsync(string id, UpdatePropertyDto updatePropertyDto, CancellationToken cancellationToken = default);
        Task<bool> DeletePropertyAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<PropertyDto>> GetPropertiesByOwnerAsync(string ownerId, CancellationToken cancellationToken = default);
        Task<IEnumerable<PropertyDto>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken = default);
        Task<IEnumerable<PropertyDto>> GetPropertiesByYearAsync(int year, CancellationToken cancellationToken = default);
    }
}
