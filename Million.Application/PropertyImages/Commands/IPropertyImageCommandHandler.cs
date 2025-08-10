using Million.Domain.DTOs;

namespace Million.Application.PropertyImages.Commands
{
    public interface IPropertyImageCommandHandler
    {
        Task<PropertyImageDto> CreatePropertyImageAsync(CreatePropertyImageDto createDto, CancellationToken cancellationToken);
        Task<PropertyImageDto> UpdatePropertyImageAsync(string id, UpdatePropertyImageDto updateDto, CancellationToken cancellationToken);
        Task<PropertyImageDto> DeletePropertyImageAsync(string id, CancellationToken cancellationToken);      
    }
}
