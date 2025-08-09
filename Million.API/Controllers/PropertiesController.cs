using Microsoft.AspNetCore.Mvc;
using Million.Application.Properties;
using Million.Domain.DTOs;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly PropertyFacade _propertyFacade;

        public PropertiesController(PropertyFacade propertyFacade)
        {
            _propertyFacade = propertyFacade;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAll()
        {
            try
            {
                var properties = await _propertyFacade.GetAllPropertiesAsync();
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDto>> GetById(string id)
        {
            try
            {
                var property = await _propertyFacade.GetPropertyByIdAsync(id);
                if (property == null)
                    return NotFound($"Property with ID {id} not found.");

                return Ok(property);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PropertyDto>> Create([FromBody] CreatePropertyDto createDto)
        {
            try
            {
                var property = await _propertyFacade.CreatePropertyAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = property.Id }, property);
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
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePropertyDto updateDto)
        {
            try
            {
                var success = await _propertyFacade.UpdatePropertyAsync(id, updateDto);
                if (!success)
                    return NotFound($"Property with ID {id} not found.");

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
                var success = await _propertyFacade.DeletePropertyAsync(id);
                if (!success)
                    return NotFound($"Property with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetByOwner(string ownerId)
        {
            try
            {
                var properties = await _propertyFacade.GetPropertiesByOwnerAsync(ownerId);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("price-range")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            try
            {
                var properties = await _propertyFacade.GetPropertiesByPriceRangeAsync(minPrice, maxPrice);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("year/{year}")]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetByYear(int year)
        {
            try
            {
                var properties = await _propertyFacade.GetPropertiesByYearAsync(year);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
