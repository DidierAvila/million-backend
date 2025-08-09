using AutoMapper;
using Microsoft.Extensions.Logging;
using Million.Domain.DTOs;
using Million.Domain.Repositories;

namespace Million.Application.Properties.Queries
{
    public class PropertyQueryHandler : IPropertyQueryHandler
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyQueryHandler> _logger;

        public PropertyQueryHandler(IPropertyRepository propertyRepository, IMapper mapper, ILogger<PropertyQueryHandler> logger)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var properties = await _propertyRepository.GetAllAsync(cancellationToken);
                return _mapper.Map<IEnumerable<PropertyDto>>(properties);
            }
            catch (Exception  ex)
            {
                var errorMessage = "An error occurred while retrieving the properties.";
                _logger.LogError(ex, errorMessage);
                return Enumerable.Empty<PropertyDto>();
            }
        }

        public async Task<PropertyDto> GetPropertyByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var property = await _propertyRepository.GetByIdAsync(id, cancellationToken);
                if (property == null)
                    return new PropertyDto() { Success = false, Messages = $"Property with ID {id} not found." };

                return _mapper.Map<PropertyDto>(property);
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while retrieving the property with ID {id}.";
                _logger.LogError(ex, errorMessage);
                return new PropertyDto() { Success = false, Messages = $"An error occurred while retrieving the property with ID {id}." };
            }
        }
    }
}
