using Microsoft.AspNetCore.Mvc;
using Million.Application.PropertyImages;
using Million.Domain.DTOs;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyImagesController : ControllerBase
    {
        private readonly PropertyImageFacade _propertyImageFacade;

        public PropertyImagesController(PropertyImageFacade propertyImageFacade)
        {
            _propertyImageFacade = propertyImageFacade;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyImageDto>> GetById(string id, CancellationToken cancellationToken)
        {
            var image = await _propertyImageFacade.GetPropertyImageByIdAsync(id, cancellationToken);
            if (!image.Success)
                return NotFound(image.Messages);

            return Ok(image);
        }

        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<PropertyImageDto>>> GetByPropertyId(string propertyId, CancellationToken cancellationToken)
        {
            try
            {
                var images = await _propertyImageFacade.GetImagesByPropertyIdAsync(propertyId, cancellationToken);
                if (images == null || !images.Any())
                    return NotFound($"No images found for property with ID {propertyId}.");

                return Ok(images);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PropertyImageDto>> Create([FromBody] CreatePropertyImageDto createDto, CancellationToken cancellationToken)
        {
            var image = await _propertyImageFacade.CreatePropertyImageAsync(createDto, cancellationToken);
            if (!image.Success)
                return BadRequest(image.Messages);

            return CreatedAtAction(nameof(GetById), new { id = image.IdPropertyImage }, image);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePropertyImageDto updateDto, CancellationToken cancellationToken)
        {
            var result = await _propertyImageFacade.UpdatePropertyImageAsync(id, updateDto, cancellationToken);
            if (!result.Success)
                return NotFound(result.Messages);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var result = await _propertyImageFacade.DeletePropertyImageAsync(id, cancellationToken);
            if (!result.Success)
                return NotFound(result.Messages);

            return NoContent();
        }
    }
}
