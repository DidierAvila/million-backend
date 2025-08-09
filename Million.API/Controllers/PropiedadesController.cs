using Microsoft.AspNetCore.Mvc;
using Million.Application.Interfaces;
using Million.Application.DTOs;
using Million.Domain.Entities;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropiedadesController : ControllerBase
    {
        private readonly IPropiedadService _propiedadService;

        public PropiedadesController(IPropiedadService propiedadService)
        {
            _propiedadService = propiedadService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Propiedad>>> GetPropiedades()
        {
            var propiedades = await _propiedadService.GetAllPropiedadesAsync();
            return Ok(propiedades);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Propiedad>> GetPropiedad(int id)
        {
            var propiedad = await _propiedadService.GetPropiedadByIdAsync(id);
            if (propiedad == null)
                return NotFound();
            
            return Ok(propiedad);
        }

        [HttpPost]
        public async Task<ActionResult<Propiedad>> CreatePropiedad(PropiedadCreateDto propiedadDto)
        {
            var propiedad = new Propiedad
            {
                Nombre = propiedadDto.Nombre,
                Direccion = propiedadDto.Direccion,
                Precio = propiedadDto.Precio,
                Impuestos = propiedadDto.Impuestos,
                Año = propiedadDto.Año,
                CodigoInterno = propiedadDto.CodigoInterno,
                IdOwner = propiedadDto.IdOwner
            };

            var createdPropiedad = await _propiedadService.CreatePropiedadAsync(propiedad);
            return CreatedAtAction(nameof(GetPropiedad), new { id = createdPropiedad.IdProperty }, createdPropiedad);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePropiedad(int id, PropiedadUpdateDto propiedadDto)
        {
            var propiedad = new Propiedad
            {
                Nombre = propiedadDto.Nombre,
                Direccion = propiedadDto.Direccion,
                Precio = propiedadDto.Precio,
                Impuestos = propiedadDto.Impuestos,
                Año = propiedadDto.Año,
                CodigoInterno = propiedadDto.CodigoInterno,
                IdOwner = propiedadDto.IdOwner
            };

            var updatedPropiedad = await _propiedadService.UpdatePropiedadAsync(id, propiedad);
            if (updatedPropiedad == null)
                return NotFound();
            
            return Ok(updatedPropiedad);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropiedad(int id)
        {
            var result = await _propiedadService.DeletePropiedadAsync(id);
            if (!result)
                return NotFound();
            
            return NoContent();
        }
    }
}