using AutoMapper;
using Million.Application.Interfaces;
using Million.Domain.DTOs;
using Million.Domain.Entities;
using Million.Domain.Repositories;

namespace Million.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertyService(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }

        public async Task<PropertyDto?> GetPropertyByIdAsync(string id)
        {
            var property = await _propertyRepository.GetByIdAsync(id);
            return property != null ? _mapper.Map<PropertyDto>(property) : null;
        }

        public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createPropertyDto)
        {
            var property = _mapper.Map<Property>(createPropertyDto);
            
            // Validaciones de negocio
            if (await _propertyRepository.ExistsAsync(p => p.InternalCode == property.InternalCode))
                throw new InvalidOperationException("A property with this internal code already exists.");

            var created = await _propertyRepository.AddAsync(property);
            return _mapper.Map<PropertyDto>(created);
        }

        public async Task<bool> UpdatePropertyAsync(string id, UpdatePropertyDto updatePropertyDto)
        {
            var existingProperty = await _propertyRepository.GetByIdAsync(id);
            if (existingProperty == null)
                return false;

            _mapper.Map(updatePropertyDto, existingProperty);
            return await _propertyRepository.UpdateAsync(id, existingProperty);
        }

        public async Task<bool> DeletePropertyAsync(string id)
        {
            return await _propertyRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PropertyDto>> GetPropertiesByOwnerAsync(string ownerId)
        {
            var properties = await _propertyRepository.GetPropertiesByOwnerAsync(ownerId);
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }

        public async Task<IEnumerable<PropertyDto>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var properties = await _propertyRepository.GetPropertiesByPriceRangeAsync(minPrice, maxPrice);
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }

        public async Task<IEnumerable<PropertyDto>> GetPropertiesByYearAsync(int year)
        {
            var properties = await _propertyRepository.GetPropertiesByYearAsync(year);
            return _mapper.Map<IEnumerable<PropertyDto>>(properties);
        }
    }
}
