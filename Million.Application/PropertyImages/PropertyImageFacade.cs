using Million.Application.PropertyImages.Commands;
using Million.Application.PropertyImages.Queries;
using Million.Domain.DTOs;

namespace Million.Application.PropertyImages
{
    public class PropertyImageFacade
    {
        private readonly IPropertyImageCommandHandler _commandHandler;
        private readonly IPropertyImageQueryHandler _queryHandler;

        public PropertyImageFacade(
            IPropertyImageCommandHandler commandHandler,
            IPropertyImageQueryHandler queryHandler)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
        }

        // Query Methods
        public async Task<IEnumerable<PropertyImageDto>> GetAllPropertyImagesAsync(CancellationToken cancellationToken)
        {
            return await _queryHandler.GetAllPropertyImagesAsync(cancellationToken);
        }

        public async Task<PropertyImageDto> GetPropertyImageByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _queryHandler.GetPropertyImageByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<PropertyImageDto>> GetImagesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken)
        {
            return await _queryHandler.GetImagesByPropertyIdAsync(propertyId, cancellationToken);
        }

        public async Task<IEnumerable<PropertyImageDto>> GetEnabledImagesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken)
        {
            return await _queryHandler.GetEnabledImagesByPropertyIdAsync(propertyId, cancellationToken);
        }

        // Command Methods
        public async Task<PropertyImageDto> CreatePropertyImageAsync(CreatePropertyImageDto createDto, CancellationToken cancellationToken)
        {
            return await _commandHandler.CreatePropertyImageAsync(createDto, cancellationToken);
        }

        public async Task<PropertyImageDto> UpdatePropertyImageAsync(string id, UpdatePropertyImageDto updateDto, CancellationToken cancellationToken)
        {
            return await _commandHandler.UpdatePropertyImageAsync(id, updateDto, cancellationToken);
        }

        public async Task<PropertyImageDto> DeletePropertyImageAsync(string id, CancellationToken cancellationToken)
        {
            return await _commandHandler.DeletePropertyImageAsync(id, cancellationToken);
        }
    }
}
