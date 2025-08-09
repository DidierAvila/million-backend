using Million.Domain.Entities;

namespace Million.Application.Interfaces
{
    public interface IPropietarioService
    {
        Task<IEnumerable<Propietario>> GetAllPropietariosAsync();
        Task<Propietario?> GetPropietarioByIdAsync(int id);
        Task<Propietario> CreatePropietarioAsync(Propietario propietario);
        Task<Propietario?> UpdatePropietarioAsync(int id, Propietario propietario);
        Task<bool> DeletePropietarioAsync(int id);
    }
}