using Million.Domain.DTOs;

namespace Million.Application.Properties.Commands
{
    public interface IPropertyCommandHandler
    {
        Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createDto, CancellationToken cancellationToken);
        Task<PropertyDto> UpdatePropertyAsync(string id, UpdatePropertyDto updateDto, CancellationToken cancellationToken  );
        Task<PropertyDto> DeletePropertyAsync(string id, CancellationToken cancellationToken);
    }
}
