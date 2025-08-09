using Microsoft.EntityFrameworkCore;
using Million.Application.Interfaces;
using Million.Domain.Entities;
using Million.Infrastructure.Data;

namespace Million.Infrastructure.Services
{
    public class PropietarioService : IPropietarioService
    {
        private readonly MillionDbContext _context;

        public PropietarioService(MillionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Propietario>> GetAllPropietariosAsync()
        {
            return await _context.Propietarios
                .Include(p => p.Propiedades)
                .ToListAsync();
        }

        public async Task<Propietario?> GetPropietarioByIdAsync(int id)
        {
            return await _context.Propietarios
                .Include(p => p.Propiedades)
                .FirstOrDefaultAsync(p => p.IdOwner == id);
        }

        public async Task<Propietario> CreatePropietarioAsync(Propietario propietario)
        {
            _context.Propietarios.Add(propietario);
            await _context.SaveChangesAsync();
            return propietario;
        }

        public async Task<Propietario?> UpdatePropietarioAsync(int id, Propietario propietario)
        {
            var existingPropietario = await _context.Propietarios.FindAsync(id);
            if (existingPropietario == null)
                return null;

            existingPropietario.Nombre = propietario.Nombre;
            existingPropietario.FechaNacimiento = propietario.FechaNacimiento;

            await _context.SaveChangesAsync();
            return existingPropietario;
        }

        public async Task<bool> DeletePropietarioAsync(int id)
        {
            var propietario = await _context.Propietarios.FindAsync(id);
            if (propietario == null)
                return false;

            _context.Propietarios.Remove(propietario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}