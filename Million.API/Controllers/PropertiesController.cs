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
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetAll(CancellationToken cancellationToken)
        {
            var properties = await _propertyFacade.GetAllPropertiesAsync(cancellationToken);
            if (properties == null || !properties.Any())
                return NotFound("No owners found.");

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
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var success = await _propertyFacade.DeletePropertyAsync(id, cancellationToken);
            if (!success.Success)
                return BadRequest(success.Messages);

            return NoContent();
        }
    }
}
