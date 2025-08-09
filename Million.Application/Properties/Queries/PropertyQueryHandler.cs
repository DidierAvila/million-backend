using AutoMapper;
using Million.Domain.DTOs;
using Million.Domain.Repositories;

namespace Million.Application.Properties.Queries
{
    public class PropertyQueryHandler : IPropertyQueryHandler
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertyQueryHandler(IPropertyRepository propertyRepository, IMapper mapper)
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
