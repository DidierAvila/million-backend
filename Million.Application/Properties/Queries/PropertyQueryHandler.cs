using AutoMapper;
using Microsoft.Extensions.Logging;
using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;

namespace Million.Application.Properties.Queries
{
    public class PropertyQueryHandler : IPropertyQueryHandler
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IOwnerRepository _iownerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyQueryHandler> _logger;

        public PropertyQueryHandler(IPropertyRepository propertyRepository, IMapper mapper, ILogger<PropertyQueryHandler> logger, IOwnerRepository iownerRepository)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _logger = logger;
            _iownerRepository = iownerRepository;
        }

        public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync(CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<Property> properties = await _propertyRepository.GetAllAsync(cancellationToken);
                if (properties.Any())
                {
                    var ownersId = properties.Select(p => p.IdOwner).Distinct().ToList();
                    IEnumerable<Owner> ownerDetails = await _iownerRepository.FindAsync(x => ownersId.Contains(x.Id), cancellationToken);
                    foreach (var property in properties)
                    {
                        var owner = ownerDetails.FirstOrDefault(x => x.Id == property.IdOwner);
                        if (owner != null)
                        {
                            property.OwnerName = owner.Name;
                        }
                    }
                }
                return _mapper.Map<IEnumerable<PropertyDto>>(properties);
            }
            catch (Exception ex)
            {
                var errorMessage = "An error occurred while retrieving the properties.";
                _logger.LogError(ex, errorMessage);
                return Enumerable.Empty<PropertyDto>();
            }
        }

        public async Task<PropertyDto> GetPropertyByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var property = await _propertyRepository.GetByIdAsync(id, cancellationToken);
                if (property == null)
                    return new PropertyDto() { Success = false, Messages = $"Property with ID {id} not found." };

                var ownerDetails = await _iownerRepository.FindAsync(x => x.Id == property.IdOwner, cancellationToken);
                var owner = ownerDetails.FirstOrDefault();
                if (owner != null)
                {
                    property.OwnerName = owner.Name;
                }
                return _mapper.Map<PropertyDto>(property);
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while retrieving the property with ID {id}.";
                _logger.LogError(ex, errorMessage);
                return new PropertyDto() { Success = false, Messages = errorMessage };
            }
        }

        public async Task<IEnumerable<PropertyDto>> GetPropertiesWithFiltersAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice, CancellationToken cancellationToken)
        {
            try
            {
                var properties = await _propertyRepository.GetPropertiesWithFiltersAsync(name, address, minPrice, maxPrice, cancellationToken);
                if (properties.Any())
                {
                    var ownersId = properties.Select(p => p.IdOwner).Distinct().ToList();
                    IEnumerable<Owner> ownerDetails = await _iownerRepository.FindAsync(x => ownersId.Contains(x.Id), cancellationToken);
                    foreach (var property in properties)
                    {
                        var owner = ownerDetails.FirstOrDefault(x => x.Id == property.IdOwner);
                        if (owner != null)
                        {
                            property.OwnerName = owner.Name;
                        }
                    }
                }
                return _mapper.Map<IEnumerable<PropertyDto>>(properties);
            }
            catch (Exception ex)
            {
                var errorMessage = "An error occurred while retrieving filtered properties.";
                _logger.LogError(ex, errorMessage);
                return Enumerable.Empty<PropertyDto>();
            }
        }
    }
}
