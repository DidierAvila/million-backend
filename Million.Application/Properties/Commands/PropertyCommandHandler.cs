using AutoMapper;
using Microsoft.Extensions.Logging;
using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;

namespace Million.Application.Properties.Commands
{
    public class PropertyCommandHandler : IPropertyCommandHandler
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyCommandHandler> _logger;


        public PropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper, ILogger<PropertyCommandHandler> logger)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                var property = _mapper.Map<Property>(createDto);

                // Validaciones de negocio
                if (await _propertyRepository.ExistsAsync(p => p.InternalCode == property.InternalCode || p.Name == property.Name, cancellationToken))
                    return new PropertyDto() { Messages = "A property with this internal code already exists.", Success = false };
                if (await _propertyRepository.ExistsAsync(p => p.Name == property.Name, cancellationToken))
                    return new PropertyDto() { Messages = "A property with this name already exists.", Success = false };
                if (await _propertyRepository.ExistsAsync(p => p.Address == property.Address, cancellationToken))
                    return new PropertyDto() { Messages = "A property with this address already exists.", Success = false };

                var created = await _propertyRepository.AddAsync(property, cancellationToken);
                return _mapper.Map<PropertyDto>(created);
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while creating the property.";
                _logger.LogError(ex, errorMessage);
                return new PropertyDto() { Success = false, Messages = "Internal server error" };
            }
        }

        public async Task<PropertyDto> UpdatePropertyAsync(string id, UpdatePropertyDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                var existingProperty = await _propertyRepository.GetByIdAsync(id, cancellationToken);
                if (existingProperty == null)
                    return new PropertyDto() { Success = false, Messages = $"Property with ID {id} not found." };

                // Validaciones de negocio
                if (await _propertyRepository.ExistsAsync(p => (p.InternalCode == updateDto.InternalCode || p.Name == updateDto.Name) && p.Id != id, cancellationToken))
                    return new PropertyDto() { Messages = "A property with this internal code already exists.", Success = false };
                if (await _propertyRepository.ExistsAsync(p => p.Name == updateDto.Name && p.Id != id, cancellationToken))
                    return new PropertyDto() { Messages = "A property with this name already exists.", Success = false };
                if (await _propertyRepository.ExistsAsync(p => p.Address == updateDto.Address && p.Id != id, cancellationToken))
                    return new PropertyDto() { Messages = "A property with this address already exists.", Success = false };

                _mapper.Map(updateDto, existingProperty);
                await _propertyRepository.UpdateAsync(id, existingProperty, cancellationToken);
                return new PropertyDto() { Success = true };
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while updating the property with ID {id}.";
                _logger.LogError(ex, errorMessage);
                return new PropertyDto() { Success = false, Messages = "Internal server error" };
            }
        }

        public async Task<PropertyDto> DeletePropertyAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var existingProperty = await _propertyRepository.GetByIdAsync(id, cancellationToken);
                if (existingProperty == null)
                    return new PropertyDto() { Success = false, Messages = $"Property with ID {id} not found." };

                await _propertyRepository.DeleteAsync(id, cancellationToken);
                return new PropertyDto() { Success = true };
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred while deleting the property with ID {id}.";
                _logger.LogError(ex, errorMessage);
                return new PropertyDto() { Success = false, Messages = "Internal server error" };
            }
        }
    }
}
