using Million.Domain.DTOs;

namespace Million.Application.Owners.Queries
{
    public interface IOwnerQueryHandler
    {
        Task<IEnumerable<OwnerDto>> GetAllOwnersAsync(CancellationToken cancellationToken = default);
        Task<OwnerDto> GetOwnerByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<OwnerDto> GetOwnerByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<OwnerDto>> GetOwnersByBirthDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<IEnumerable<OwnerDto>> GetOwnersByNameContainingAsync(string name, CancellationToken cancellationToken = default);
    }
}