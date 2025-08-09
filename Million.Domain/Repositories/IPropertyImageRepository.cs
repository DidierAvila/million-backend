using Million.Domain.Entities;

namespace Million.Domain.Repositories
{
    public interface IPropertyImageRepository : IRepository<PropertyImage>
    {
        Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(string propertyId);
        Task<IEnumerable<PropertyImage>> GetEnabledImagesByPropertyIdAsync(string propertyId);
    }
}
