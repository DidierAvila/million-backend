using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAll(
            [FromQuery] string? name = null,
            [FromQuery] string? address = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            CancellationToken cancellationToken = default)
        {
            IEnumerable<PropertyDto> properties = await _propertyFacade.GetPropertiesWithFiltersAsync(name, address, minPrice, maxPrice, cancellationToken);
            return Ok(properties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDto>> GetById(string id, CancellationToken cancellationToken)
        {
            var property = await _propertyFacade.GetPropertyByIdAsync(id, cancellationToken);
            if (!property.Success)
                return NotFound(property.Messages);

            return Ok(property);
        }

        [HttpPost]
        public async Task<ActionResult<PropertyDto>> Create([FromBody] CreatePropertyDto createDto, CancellationToken cancellationToken)
        {
            var property = await _propertyFacade.CreatePropertyAsync(createDto, cancellationToken);
            if (!property.Success)
                return BadRequest(property.Messages);

            return CreatedAtAction(nameof(GetById), new { id = property.Id }, property);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePropertyDto updateDto, CancellationToken cancellationToken)
        {
            var success = await _propertyFacade.UpdatePropertyAsync(id, updateDto, cancellationToken);
            if (!success.Success)
                return BadRequest(success.Messages);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var success = await _propertyFacade.DeletePropertyAsync(id, cancellationToken);
            if (!success.Success)
                return BadRequest(success.Messages);

            return NoContent();
        }
    }
}
