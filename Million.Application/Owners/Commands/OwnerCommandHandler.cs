using AutoMapper;
using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;

namespace Million.Application.Owners.Commands
{
    public interface IOwnerCommandHandler
    {
        Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createDto);
        Task<bool> UpdateOwnerAsync(string id, UpdateOwnerDto updateDto);
        Task<bool> DeleteOwnerAsync(string id);
    }

    public class OwnerCommandHandler : IOwnerCommandHandler
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;

        public OwnerCommandHandler(IOwnerRepository ownerRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        public async Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createDto)
        {
            var owner = _mapper.Map<Owner>(createDto);
            
            if (await _ownerRepository.ExistsAsync(o => o.Name == owner.Name))
                throw new InvalidOperationException("An owner with this name already exists.");

            var created = await _ownerRepository.AddAsync(owner);
            return _mapper.Map<OwnerDto>(created);
        }

        public async Task<bool> UpdateOwnerAsync(string id, UpdateOwnerDto updateDto)
        {
            var existingOwner = await _ownerRepository.GetByIdAsync(id);
            if (existingOwner == null)
                return false;

            _mapper.Map(updateDto, existingOwner);
            return await _ownerRepository.UpdateAsync(id, existingOwner);
        }

        public async Task<bool> DeleteOwnerAsync(string id)
        {
            return await _ownerRepository.DeleteAsync(id);
        }
    }
}
