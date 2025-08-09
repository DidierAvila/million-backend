using AutoMapper;
using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;

namespace Million.Application.Properties.Commands
{
    public class PropertyCommandHandler : IPropertyCommandHandler
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertyCommandHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createDto)
        {
            var property = _mapper.Map<Property>(createDto);
            
            // Validaciones de negocio
            if (await _propertyRepository.ExistsAsync(p => p.InternalCode == property.InternalCode))
                throw new InvalidOperationException("A property with this internal code already exists.");

            var created = await _propertyRepository.AddAsync(property);
            return _mapper.Map<PropertyDto>(created);
        }

        public async Task<bool> UpdatePropertyAsync(string id, UpdatePropertyDto updateDto)
        {
            var existingProperty = await _propertyRepository.GetByIdAsync(id);
            if (existingProperty == null)
                return false;

            _mapper.Map(updateDto, existingProperty);
            return await _propertyRepository.UpdateAsync(id, existingProperty);
        }

        public async Task<bool> DeletePropertyAsync(string id)
        {
            return await _propertyRepository.DeleteAsync(id);
        }
    }
}
