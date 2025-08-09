using Million.Application.Properties.Commands;
using Million.Application.Properties.Queries;
using Million.Domain.DTOs;

namespace Million.Application.Properties
{
    public class PropertyFacade
    {
        private readonly IPropertyCommandHandler _commandHandler;
        private readonly IPropertyQueryHandler _queryHandler;

        public PropertyFacade(
            IPropertyCommandHandler commandHandler,
            IPropertyQueryHandler queryHandler)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
        }

        // Query Methods
        public Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync() =>
            _queryHandler.GetAllPropertiesAsync();

        public Task<PropertyDto?> GetPropertyByIdAsync(string id) =>
            _queryHandler.GetPropertyByIdAsync(id);

        public Task<IEnumerable<PropertyDto>> GetPropertiesByOwnerAsync(string ownerId) =>
            _queryHandler.GetPropertiesByOwnerAsync(ownerId);

        public Task<IEnumerable<PropertyDto>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice) =>
            _queryHandler.GetPropertiesByPriceRangeAsync(minPrice, maxPrice);

        public Task<IEnumerable<PropertyDto>> GetPropertiesByYearAsync(int year) =>
            _queryHandler.GetPropertiesByYearAsync(year);

        // Command Methods
        public Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createDto) =>
            _commandHandler.CreatePropertyAsync(createDto);

        public Task<bool> UpdatePropertyAsync(string id, UpdatePropertyDto updateDto) =>
            _commandHandler.UpdatePropertyAsync(id, updateDto);

        public Task<bool> DeletePropertyAsync(string id) =>
            _commandHandler.DeletePropertyAsync(id);
    }
}
