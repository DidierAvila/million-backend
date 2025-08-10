using AutoMapper;
using Microsoft.Extensions.Logging;
using Million.Domain.DTOs;
using Million.Domain.Repositories;

namespace Million.Application.PropertyTraces.Queries
{
    public class PropertyTraceQueryHandler : IPropertyTraceQueryHandler
    {
        private readonly IPropertyTraceRepository _propertyTraceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyTraceQueryHandler> _logger;

        public PropertyTraceQueryHandler(IPropertyTraceRepository propertyTraceRepository, IMapper mapper, ILogger<PropertyTraceQueryHandler> logger)
        {
            _propertyTraceRepository = propertyTraceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PropertyTraceDto>> GetAllPropertyTracesAsync(CancellationToken cancellationToken = default)
        {
            var traces = await _propertyTraceRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PropertyTraceDto>>(traces);
        }

        public async Task<PropertyTraceDto> GetPropertyTraceByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var trace = await _propertyTraceRepository.GetByIdAsync(id, cancellationToken);
                if (trace == null)
                    return new PropertyTraceDto
                    { Messages = $"Property trace with ID {id} not found.", Success = false };

                return _mapper.Map<PropertyTraceDto>(trace);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving property trace with ID {Id}", id);
                return new PropertyTraceDto
                { Messages = $"Error retrieving property trace with ID: {id}", Success = false };
            }
        }

        public async Task<IEnumerable<PropertyTraceDto>> GetTracesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken = default)
        {
            var traces = await _propertyTraceRepository.GetTracesByPropertyIdAsync(propertyId, cancellationToken);
            return _mapper.Map<IEnumerable<PropertyTraceDto>>(traces);
        }

        public async Task<IEnumerable<PropertyTraceDto>> GetTracesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var traces = await _propertyTraceRepository.GetTracesByDateRangeAsync(startDate, endDate, cancellationToken);
            return _mapper.Map<IEnumerable<PropertyTraceDto>>(traces);
        }
    }
}
