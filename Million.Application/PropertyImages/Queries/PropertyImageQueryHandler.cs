using AutoMapper;
using Microsoft.Extensions.Logging;
using Million.Domain.DTOs;
using Million.Domain.Repositories;

namespace Million.Application.PropertyImages.Queries
{
    public class PropertyImageQueryHandler : IPropertyImageQueryHandler
    {
        private readonly IPropertyImageRepository _propertyImageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyImageQueryHandler> _logger;

        public PropertyImageQueryHandler(IPropertyImageRepository propertyImageRepository, IMapper mapper, ILogger<PropertyImageQueryHandler> logger)
        {
            _propertyImageRepository = propertyImageRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PropertyImageDto>> GetAllPropertyImagesAsync(CancellationToken cancellationToken)
        {
            var images = await _propertyImageRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PropertyImageDto>>(images);
        }

        public async Task<PropertyImageDto> GetPropertyImageByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var image = await _propertyImageRepository.GetByIdAsync(id, cancellationToken);
                if (image == null)
                    return new PropertyImageDto() { Messages = $"Property image with ID {id} not found.", Success = false };

                return _mapper.Map<PropertyImageDto>(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving property image with ID {Id}", id);
                return new PropertyImageDto() { Messages = $"Error retrieving property image with ID: {id}", Success = false };
            }
        }

        public async Task<IEnumerable<PropertyImageDto>> GetImagesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken)
        {
            try
            {
                var images = await _propertyImageRepository.GetImagesByPropertyIdAsync(propertyId, cancellationToken);
                return _mapper.Map<IEnumerable<PropertyImageDto>>(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving property image with PropertyId: {PropertyId}", propertyId);
                return [];
            }
        }

        public async Task<IEnumerable<PropertyImageDto>> GetEnabledImagesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken)
        {
            var images = await _propertyImageRepository.GetEnabledImagesByPropertyIdAsync(propertyId, cancellationToken);
            return _mapper.Map<IEnumerable<PropertyImageDto>>(images);
        }
    }
}
