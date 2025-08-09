using Million.Domain.DTOs;

namespace Million.Application.Interfaces
{
    public interface IOwnerService
    {
        Task<IEnumerable<OwnerDto>> GetAllOwnersAsync();
        Task<OwnerDto?> GetOwnerByIdAsync(string id);
        Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createOwnerDto);
        Task<bool> UpdateOwnerAsync(string id, UpdateOwnerDto updateOwnerDto);
        Task<bool> DeleteOwnerAsync(string id);
        Task<OwnerDto?> GetOwnerByNameAsync(string name);
        Task<IEnumerable<OwnerDto>> GetOwnersByBirthDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
