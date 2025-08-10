using Million.Domain.Entities;

namespace Million.Domain.Repositories
{
    public interface IPropertyImageRepository : IRepository<PropertyImage>
    {
        Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken);
        Task<IEnumerable<PropertyImage>> GetEnabledImagesByPropertyIdAsync(string propertyId, CancellationToken cancellationToken);
    }
}
