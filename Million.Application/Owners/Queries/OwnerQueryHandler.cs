using AutoMapper;
using Million.Domain.DTOs;
using Million.Domain.Repositories;

namespace Million.Application.Owners.Queries
{
    public interface IOwnerQueryHandler
    {
        Task<IEnumerable<OwnerDto>> GetAllOwnersAsync();
        Task<OwnerDto?> GetOwnerByIdAsync(string id);
        Task<OwnerDto?> GetOwnerByNameAsync(string name);
        Task<IEnumerable<OwnerDto>> GetOwnersByBirthDateRangeAsync(DateTime startDate, DateTime endDate);
    }

    public class OwnerQueryHandler : IOwnerQueryHandler
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public OwnerQueryHandler(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OwnerDto>> GetAllOwnersAsync()
        {
            var owners = await _ownerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OwnerDto>>(owners);
        }

        public async Task<OwnerDto?> GetOwnerByIdAsync(string id)
        {
            var owner = await _ownerRepository.GetByIdAsync(id);
            return owner != null ? _mapper.Map<OwnerDto>(owner) : null;
        }

        public async Task<OwnerDto?> GetOwnerByNameAsync(string name)
        {
            var owner = await _ownerRepository.GetOwnerByNameAsync(name);
            return owner != null ? _mapper.Map<OwnerDto>(owner) : null;
        }

        public async Task<IEnumerable<OwnerDto>> GetOwnersByBirthDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var owners = await _ownerRepository.GetOwnersByBirthDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<OwnerDto>>(owners);
        }
    }
}
