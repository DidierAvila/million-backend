using AutoMapper;
using Microsoft.Extensions.Logging;
using Million.Domain.DTOs;
using Million.Domain.Repositories;

namespace Million.Application.Owners.Queries
{

    public class OwnerQueryHandler : IOwnerQueryHandler
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OwnerQueryHandler> _logger;

        public OwnerQueryHandler(IOwnerRepository ownerRepository, IMapper mapper, ILogger<OwnerQueryHandler> logger)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<OwnerDto>> GetAllOwnersAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var owners = await _ownerRepository.GetAllAsync(cancellationToken);
                return _mapper.Map<IEnumerable<OwnerDto>>(owners);
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while retrieving the owners.";
                _logger.LogError(ex, errorMessage);
                return (IEnumerable<OwnerDto>)Array.Empty<object>();
            }
        }

        public async Task<OwnerDto> GetOwnerByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var owner = await _ownerRepository.GetByIdAsync(id, cancellationToken);
                if (owner == null)
                    return new OwnerDto { Messages = $"Owner with ID {id} not found.", Success = false };

                return _mapper.Map<OwnerDto>(owner);
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while retrieving the owner with ID {id}.";
                _logger.LogError(ex, errorMessage);
                return new OwnerDto { Messages = $"Internal server error", Success = false };
            }
        }

        public async Task<OwnerDto> GetOwnerByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var owner = await _ownerRepository.GetOwnerByNameAsync(name, cancellationToken);
            if (owner == null)
                return new OwnerDto { Messages = $"Owner with name {name} not found.", Success = false };
            return _mapper.Map<OwnerDto>(owner);
        }

        public async Task<IEnumerable<OwnerDto>> GetOwnersByBirthDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var owners = await _ownerRepository.GetOwnersByBirthDateRangeAsync(startDate, endDate, cancellationToken);
            return _mapper.Map<IEnumerable<OwnerDto>>(owners);
        }

        public async Task<IEnumerable<OwnerDto>> GetOwnersByNameContainingAsync(string name, CancellationToken cancellationToken = default)
        {
            try
            {
                var owners = await _ownerRepository.GetOwnersByNameContainingAsync(name, cancellationToken);
                return _mapper.Map<IEnumerable<OwnerDto>>(owners);
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while retrieving owners with name containing '{name}'.";
                _logger.LogError(ex, errorMessage);
                return Array.Empty<OwnerDto>();
            }
        }
    }
}
