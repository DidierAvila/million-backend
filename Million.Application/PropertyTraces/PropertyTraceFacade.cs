using Million.Application.PropertyTraces.Commands;
using Million.Application.PropertyTraces.Queries;
using Million.Domain.DTOs;

namespace Million.Application.PropertyTraces
{
    public class PropertyTraceFacade
    {
        private readonly IPropertyTraceCommandHandler _commandHandler;
        private readonly IPropertyTraceQueryHandler _queryHandler;

        public PropertyTraceFacade(
            IPropertyTraceCommandHandler commandHandler,
            IPropertyTraceQueryHandler queryHandler)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
        }

        // Query Methods
        public Task<IEnumerable<PropertyTraceDto>> GetAllPropertyTracesAsync(CancellationToken cancellationToken) =>
            _queryHandler.GetAllPropertyTracesAsync(cancellationToken);

        public Task<PropertyTraceDto> GetPropertyTraceByIdAsync(string id, CancellationToken cancellationToken) =>
            _queryHandler.GetPropertyTraceByIdAsync(id, cancellationToken);

        public Task<IEnumerable<PropertyTraceDto>> GetTracesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken) =>
            _queryHandler.GetTracesByPropertyIdAsync(propertyId, cancellationToken);

        public Task<IEnumerable<PropertyTraceDto>> GetTracesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken) =>
            _queryHandler.GetTracesByDateRangeAsync(startDate, endDate, cancellationToken);

        // Command Methods
        public Task<PropertyTraceDto> CreatePropertyTraceAsync(CreatePropertyTraceDto createDto, CancellationToken cancellationToken) =>
            _commandHandler.CreatePropertyTraceAsync(createDto, cancellationToken);

        public Task<bool> DeletePropertyTraceAsync(string id, CancellationToken cancellationToken) =>
            _commandHandler.DeletePropertyTraceAsync(id, cancellationToken);
    }
}
