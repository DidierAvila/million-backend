using AutoMapper;
using Microsoft.Extensions.Logging;
using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;

namespace Million.Application.Owners.Commands
{
    public class OwnerCommandHandler : IOwnerCommandHandler
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OwnerCommandHandler> _logger;

        public OwnerCommandHandler(IOwnerRepository ownerRepository, IMapper mapper, ILogger<OwnerCommandHandler> logger)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                var owner = _mapper.Map<Owner>(createDto);

                if (await _ownerRepository.ExistsAsync(o => o.Name == owner.Name, cancellationToken))
                    return new OwnerDto() { Messages = "An owner with this name already exists.", Success = false };

                var created = await _ownerRepository.AddAsync(owner, cancellationToken);
                return _mapper.Map<OwnerDto>(created);
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while retrieving the owners.";
                _logger.LogError(ex, errorMessage);
                return new OwnerDto() { Success = false, Messages = "Internal server error" };
            }
        }

        public async Task<OwnerDto> UpdateOwnerAsync(string id, UpdateOwnerDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                var existingOwner = await _ownerRepository.GetByIdAsync(id, cancellationToken);
                if (existingOwner == null)
                    return new OwnerDto() { Success = false, Messages = $"Owner with ID {id} not found." };

                _mapper.Map(updateDto, existingOwner);
                await _ownerRepository.UpdateAsync(id, existingOwner, cancellationToken);

                return new OwnerDto() { Success = true };
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while updating the owner with ID {id}.";
                _logger.LogError(ex, errorMessage);
                return new OwnerDto() { Success = false, Messages = "Internal server error" };
            }
        }

        public async Task<OwnerDto> DeleteOwnerAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var existingOwner = await _ownerRepository.GetByIdAsync(id, cancellationToken);
                if (existingOwner == null)
                    return new OwnerDto() { Success = false, Messages = $"Owner with ID {id} not found." };

                var result = await _ownerRepository.DeleteAsync(id, cancellationToken);
                return result ? new OwnerDto() { Success = true } : new OwnerDto() { Success = false, Messages = $"An error occurred while deleting the owner with ID {id}." };
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while deleting the owner with ID {id}.";
                _logger.LogError(ex, errorMessage);
                return new OwnerDto() { Success = false, Messages = "Internal server error" };
            }
        }
    }
}
