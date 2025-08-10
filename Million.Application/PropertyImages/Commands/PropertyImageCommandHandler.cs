using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Million.Application.Owners.Queries;
using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;

namespace Million.Application.PropertyImages.Commands
{
    public class PropertyImageCommandHandler : IPropertyImageCommandHandler
    {
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyImageCommandHandler> _logger;

        public PropertyImageCommandHandler(
            IPropertyImageRepository propertyImageRepository,
            IPropertyRepository propertyRepository,
            IMapper mapper,
            ILogger<PropertyImageCommandHandler> logger)
        {
            _propertyImageRepository = propertyImageRepository;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PropertyImageDto> CreatePropertyImageAsync(CreatePropertyImageDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                // Verificar que la propiedad existe
                if (!await _propertyRepository.ExistsAsync(p => p.Id == createDto.IdProperty, cancellationToken))
                    return new PropertyImageDto() { Messages = $"Property with ID {createDto.IdProperty} not found.", Success = false };

                if (createDto.File.Length == 0)
                    return new PropertyImageDto() { Messages = "Image cannot be empty.", Success = false };

                var propertyImage = _mapper.Map<PropertyImage>(createDto);
                var created = await _propertyImageRepository.AddAsync(propertyImage, cancellationToken);
                return _mapper.Map<PropertyImageDto>(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating property image");
                return new PropertyImageDto()
                {
                    Messages = "An error occurred while creating the property image.",
                    Success = false
                };
            }
        }

        public async Task<PropertyImageDto> UpdatePropertyImageAsync(string id, UpdatePropertyImageDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!await _propertyRepository.ExistsAsync(p => p.Id == id, cancellationToken))
                    return new PropertyImageDto() { Messages = $"Property with ID {id} not found.", Success = false };

                var existingImage = await _propertyImageRepository.GetByIdAsync(id, cancellationToken);
                if (existingImage == null)
                    return new PropertyImageDto() { Messages = $"Property image with ID {id} not found.", Success = false };

                _mapper.Map(updateDto, existingImage);
                await _propertyImageRepository.UpdateAsync(id, existingImage, cancellationToken);
                return new PropertyImageDto() { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating property image");
                return new PropertyImageDto()
                {
                    Messages = "An error occurred while updating the property image.",
                    Success = false
                };
            }
        }

        public async Task<PropertyImageDto> DeletePropertyImageAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var existingImage = await _propertyImageRepository.GetByIdAsync(id, cancellationToken);
                if (existingImage == null)
                    return new PropertyImageDto() { Messages = $"Property image with ID {id} not found.", Success = false };

                await _propertyImageRepository.DeleteAsync(id, cancellationToken);
                return new PropertyImageDto() { Success = true, Messages = "Property image deleted successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting property image");
                return new PropertyImageDto()
                {
                    Messages = "An error occurred while deleting the property image.",
                    Success = false
                };
            }
        }
    }
}
