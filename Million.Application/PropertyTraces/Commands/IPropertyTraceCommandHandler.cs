using Million.Domain.DTOs;

namespace Million.Application.PropertyTraces.Commands
{
    public interface IPropertyTraceCommandHandler
    {
        Task<PropertyTraceDto> CreatePropertyTraceAsync(CreatePropertyTraceDto createDto, CancellationToken cancellationToken = default);
        Task<bool> UpdatePropertyTraceAsync(string id, UpdatePropertyTraceDto updateDto, CancellationToken cancellationToken = default);
        Task<bool> DeletePropertyTraceAsync(string id, CancellationToken cancellationToken = default);
    }
}
