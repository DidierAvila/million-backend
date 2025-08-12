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
        public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync(CancellationToken cancellationToken) =>
            await _queryHandler.GetAllPropertiesAsync(cancellationToken);

        public async Task<PropertyDto> GetPropertyByIdAsync(string id, CancellationToken cancellationToken) =>
            await _queryHandler.GetPropertyByIdAsync(id, cancellationToken);

        public async Task<IEnumerable<PropertyDto>> GetPropertiesWithFiltersAsync(string? name, string? address, decimal? minPrice,
            decimal? maxPrice, CancellationToken cancellationToken) =>
            await _queryHandler.GetPropertiesWithFiltersAsync(name, address, minPrice, maxPrice, cancellationToken);

        // Command Methods
        public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createDto, CancellationToken cancellationToken) =>
            await _commandHandler.CreatePropertyAsync(createDto, cancellationToken);

        public async Task<PropertyDto> UpdatePropertyAsync(string id, UpdatePropertyDto updateDto, CancellationToken cancellationToken) =>
            await _commandHandler.UpdatePropertyAsync(id, updateDto, cancellationToken);

        public async Task<PropertyDto> DeletePropertyAsync(string id, CancellationToken cancellationToken) =>
            await _commandHandler.DeletePropertyAsync(id, cancellationToken);
    }
}
