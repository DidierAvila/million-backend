using Million.Domain.DTOs;

namespace Million.Application.Properties.Commands
{
    public interface IPropertyCommandHandler
    {
        Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createDto);
        Task<bool> UpdatePropertyAsync(string id, UpdatePropertyDto updateDto);
        Task<bool> DeletePropertyAsync(string id);
    }
}
