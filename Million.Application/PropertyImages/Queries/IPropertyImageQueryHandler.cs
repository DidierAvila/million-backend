using Million.Domain.DTOs;

namespace Million.Application.PropertyImages.Queries
{
    public interface IPropertyImageQueryHandler
    {
        Task<IEnumerable<PropertyImageDto>> GetAllPropertyImagesAsync(CancellationToken cancellationToken);
        Task<PropertyImageDto> GetPropertyImageByIdAsync(string id, CancellationToken cancellationToken);
        Task<IEnumerable<PropertyImageDto>> GetImagesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken);
        Task<IEnumerable<PropertyImageDto>> GetEnabledImagesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken);
    }
}
