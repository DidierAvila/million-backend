using Million.Domain.DTOs;

namespace Million.Application.Owners.Commands
{
    public interface IOwnerCommandHandler
    {
        Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createDto, CancellationToken cancellationToken);
        Task<OwnerDto> UpdateOwnerAsync(string id, UpdateOwnerDto updateDto, CancellationToken cancellationToken);
        Task<OwnerDto> DeleteOwnerAsync(string id, CancellationToken cancellationToken);
    }
}