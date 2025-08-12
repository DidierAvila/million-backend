using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Million.Application.PropertyTraces
{
    public class PropertyTraceService
    {
        private readonly IPropertyTraceRepository _propertyTraceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PropertyTraceService> _logger;

        public PropertyTraceService(
            IPropertyTraceRepository propertyTraceRepository,
            IMapper mapper,
            ILogger<PropertyTraceService> logger)
        {
            _propertyTraceRepository = propertyTraceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PropertyTraceDto> CreateTraceAsync(CreatePropertyTraceDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                var trace = _mapper.Map<PropertyTrace>(createDto);
                trace.Date = DateTime.UtcNow;

                var createdTrace = await _propertyTraceRepository.AddAsync(trace, cancellationToken);
                return _mapper.Map<PropertyTraceDto>(createdTrace);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating property trace");
                return new PropertyTraceDto { 
                    Success = false, 
                    Messages = "Error creating property trace: " + ex.Message 
                };
            }
        }

        public async Task<IEnumerable<PropertyTraceDto>> GetTracesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken)
        {
            var traces = await _propertyTraceRepository.GetTracesByPropertyIdAsync(propertyId, cancellationToken);
            return _mapper.Map<IEnumerable<PropertyTraceDto>>(traces);
        }
    }
}
