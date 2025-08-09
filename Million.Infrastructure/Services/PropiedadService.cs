using Microsoft.EntityFrameworkCore;
using Million.Application.Interfaces;
using Million.Domain.Entities;
using Million.Infrastructure.Data;

namespace Million.Infrastructure.Services
{
    public class PropiedadService : IPropiedadService
    {
        private readonly MillionDbContext _context;

        public PropiedadService(MillionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Propiedad>> GetAllPropiedadesAsync()
        {
            return await _context.Propiedades
                .Include(p => p.Propietario)
                .Include(p => p.ImagenesPropiedad)
                .Include(p => p.TrazabilidadesPropiedad)
                .ToListAsync();
        }

        public async Task<Propiedad?> GetPropiedadByIdAsync(int id)
        {
            return await _context.Propiedades
                .Include(p => p.Propietario)
                .Include(p => p.ImagenesPropiedad)
                .Include(p => p.TrazabilidadesPropiedad)
                .FirstOrDefaultAsync(p => p.IdProperty == id);
        }

        public async Task<Propiedad> CreatePropiedadAsync(Propiedad propiedad)
        {
            _context.Propiedades.Add(propiedad);
            await _context.SaveChangesAsync();
            return propiedad;
        }

        public async Task<Propiedad?> UpdatePropiedadAsync(int id, Propiedad propiedad)
        {
            var existingPropiedad = await _context.Propiedades.FindAsync(id);
            if (existingPropiedad == null)
                return null;

            existingPropiedad.Nombre = propiedad.Nombre;
            existingPropiedad.Direccion = propiedad.Direccion;
            existingPropiedad.Precio = propiedad.Precio;
            existingPropiedad.Impuestos = propiedad.Impuestos;
            existingPropiedad.Año = propiedad.Año;
            existingPropiedad.CodigoInterno = propiedad.CodigoInterno;
            existingPropiedad.IdOwner = propiedad.IdOwner;

            await _context.SaveChangesAsync();
            return existingPropiedad;
        }

        public async Task<bool> DeletePropiedadAsync(int id)
        {
            var propiedad = await _context.Propiedades.FindAsync(id);
            if (propiedad == null)
                return false;

            _context.Propiedades.Remove(propiedad);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}