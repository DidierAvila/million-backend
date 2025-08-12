using AutoMapper;
using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;

namespace Million.Application.PropertyTraces.Commands
{
    public class PropertyTraceCommandHandler : IPropertyTraceCommandHandler
    {
        private readonly IPropertyTraceRepository _propertyTraceRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertyTraceCommandHandler(
            IPropertyTraceRepository propertyTraceRepository,
            IPropertyRepository propertyRepository,
            IMapper mapper)
        {
            _propertyTraceRepository = propertyTraceRepository;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<PropertyTraceDto> CreatePropertyTraceAsync(CreatePropertyTraceDto createDto, CancellationToken cancellationToken)
        {
            // Verificar que la propiedad existe
            if (!await _propertyRepository.ExistsAsync(p => p.Id == createDto.PropertyId, cancellationToken))
                throw new InvalidOperationException($"Property with ID {createDto.PropertyId} not found.");

            var propertyTrace = _mapper.Map<PropertyTrace>(createDto);
            var created = await _propertyTraceRepository.AddAsync(propertyTrace, cancellationToken);
            return _mapper.Map<PropertyTraceDto>(created);
        }

        public async Task<bool> DeletePropertyTraceAsync(string id, CancellationToken cancellationToken)
        {
            return await _propertyTraceRepository.DeleteAsync(id, cancellationToken);
        }
    }
}
