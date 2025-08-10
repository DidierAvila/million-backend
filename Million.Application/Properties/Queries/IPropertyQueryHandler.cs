using Million.Domain.DTOs;

namespace Million.Application.Properties.Queries
{
    public interface IPropertyQueryHandler
    {
        Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync(CancellationToken cancellationToken);
        Task<PropertyDto> GetPropertyByIdAsync(string id, CancellationToken cancellationToken);
    }
}
