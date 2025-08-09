using Million.Domain.Entities;

namespace Million.Application.Interfaces
{
    public interface IPropiedadService
    {
        Task<IEnumerable<Propiedad>> GetAllPropiedadesAsync();
        Task<Propiedad?> GetPropiedadByIdAsync(int id);
        Task<Propiedad> CreatePropiedadAsync(Propiedad propiedad);
        Task<Propiedad?> UpdatePropiedadAsync(int id, Propiedad propiedad);
        Task<bool> DeletePropiedadAsync(int id);
    }
}