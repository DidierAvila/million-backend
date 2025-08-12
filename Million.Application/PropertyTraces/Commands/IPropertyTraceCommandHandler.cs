using Million.Domain.DTOs;

namespace Million.Application.PropertyTraces.Commands
{
    public interface IPropertyTraceCommandHandler
    {
        Task<PropertyTraceDto> CreatePropertyTraceAsync(CreatePropertyTraceDto createDto, CancellationToken cancellationToken);
        Task<bool> DeletePropertyTraceAsync(string id, CancellationToken cancellationToken);
    }
}
