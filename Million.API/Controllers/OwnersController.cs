using Microsoft.AspNetCore.Mvc;
using Million.Application.Owners;
using Million.Domain.DTOs;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly OwnerFacade _ownerFacade;

        public OwnersController(OwnerFacade ownerFacade)
        {
            _ownerFacade = ownerFacade;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetAll()
        {
            try
            {
                var owners = await _ownerFacade.GetAllOwnersAsync();
                return Ok(owners);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerDto>> GetById(string id)
        {
            try
            {
                var owner = await _ownerFacade.GetOwnerByIdAsync(id);
                if (owner == null)
                    return NotFound($"Owner with ID {id} not found.");

                return Ok(owner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<OwnerDto>> GetByName(string name)
        {
            try
            {
                var owner = await _ownerFacade.GetOwnerByNameAsync(name);
                if (owner == null)
                    return NotFound($"Owner with name {name} not found.");

                return Ok(owner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("birth-date-range")]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetByBirthDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var owners = await _ownerFacade.GetOwnersByBirthDateRangeAsync(startDate, endDate);
                return Ok(owners);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<OwnerDto>> Create([FromBody] CreateOwnerDto createDto)
        {
            try
            {
                var owner = await _ownerFacade.CreateOwnerAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = owner.Id }, owner);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateOwnerDto updateDto)
        {
            try
            {
                var success = await _ownerFacade.UpdateOwnerAsync(id, updateDto);
                if (!success)
                    return NotFound($"Owner with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var success = await _ownerFacade.DeleteOwnerAsync(id);
                if (!success)
                    return NotFound($"Owner with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
