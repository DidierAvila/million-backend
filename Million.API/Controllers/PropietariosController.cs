using Microsoft.AspNetCore.Mvc;
using Million.Application.Interfaces;
using Million.Application.DTOs;
using Million.Domain.Entities;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropietariosController : ControllerBase
    {
        private readonly IPropietarioService _propietarioService;

        public PropietariosController(IPropietarioService propietarioService)
        {
            _propietarioService = propietarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Propietario>>> GetPropietarios()
        {
            var propietarios = await _propietarioService.GetAllPropietariosAsync();
            return Ok(propietarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Propietario>> GetPropietario(int id)
        {
            var propietario = await _propietarioService.GetPropietarioByIdAsync(id);
            if (propietario == null)
                return NotFound();
            
            return Ok(propietario);
        }

        [HttpPost]
        public async Task<ActionResult<Propietario>> CreatePropietario(PropietarioCreateDto propietarioDto)
        {
            var propietario = new Propietario
            {
                Nombre = propietarioDto.Nombre,
                FechaNacimiento = propietarioDto.FechaNacimiento
            };

            var createdPropietario = await _propietarioService.CreatePropietarioAsync(propietario);
            return CreatedAtAction(nameof(GetPropietario), new { id = createdPropietario.IdOwner }, createdPropietario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePropietario(int id, PropietarioUpdateDto propietarioDto)
        {
            var propietario = new Propietario
            {
                Nombre = propietarioDto.Nombre,
                FechaNacimiento = propietarioDto.FechaNacimiento
            };

            var updatedPropietario = await _propietarioService.UpdatePropietarioAsync(id, propietario);
            if (updatedPropietario == null)
                return NotFound();
            
            return Ok(updatedPropietario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropietario(int id)
        {
            var result = await _propietarioService.DeletePropietarioAsync(id);
            if (!result)
                return NotFound();
            
            return NoContent();
        }
    }
}