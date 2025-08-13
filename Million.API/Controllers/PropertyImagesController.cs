using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Million.Application.PropertyImages;
using Million.Application.Services;
using Million.Domain.DTOs;
using System.IO;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyImagesController : ControllerBase
    {
        private readonly PropertyImageFacade _propertyImageFacade;
        private readonly IS3Service _s3Service;

        public PropertyImagesController(PropertyImageFacade propertyImageFacade, IS3Service s3Service)
        {
            _propertyImageFacade = propertyImageFacade;
            _s3Service = s3Service;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<PropertyImageDto>> GetById(string id, CancellationToken cancellationToken)
        {
            var image = await _propertyImageFacade.GetPropertyImageByIdAsync(id, cancellationToken);
            if (!image.Success)
                return NotFound(image.Messages);

            return Ok(image);
        }

        [HttpGet("property/{propertyId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<PropertyImageDto>>> GetByPropertyId(string propertyId, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<PropertyImageDto> images = await _propertyImageFacade.GetImagesByPropertyIdAsync(propertyId, cancellationToken);
                return Ok(images);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<PropertyImageDto>> Create([FromForm] CreatePropertyImageDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                if (createDto.ImageFile != null)
                {
                    // Generate a unique filename to avoid conflicts
                    string fileExtension = Path.GetExtension(createDto.ImageFile.FileName);
                    string fileName = $"{Guid.NewGuid()}{fileExtension}";
                    string key = $"properties/{createDto.IdProperty}/{fileName}";

                    // Upload the file to S3
                    string fileUrl = await _s3Service.UploadFileAsync(createDto.ImageFile, key);
                    
                    // Set the URL as the File property
                    createDto.File = fileUrl;
                }

                var image = await _propertyImageFacade.CreatePropertyImageAsync(createDto, cancellationToken);
                if (!image.Success)
                    return BadRequest(image.Messages);

                return CreatedAtAction(nameof(GetById), new { id = image.IdPropertyImage }, image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading image: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Update(string id, [FromForm] UpdatePropertyImageDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                // First, get the existing image to know the property ID
                var existingImage = await _propertyImageFacade.GetPropertyImageByIdAsync(id, cancellationToken);
                if (!existingImage.Success)
                    return NotFound(existingImage.Messages);

                if (updateDto.ImageFile != null)
                {
                    // Generate a unique filename to avoid conflicts
                    string fileExtension = Path.GetExtension(updateDto.ImageFile.FileName);
                    string fileName = $"{Guid.NewGuid()}{fileExtension}";
                    string key = $"properties/{existingImage.IdProperty}/{fileName}";

                    // Upload the file to S3
                    string fileUrl = await _s3Service.UploadFileAsync(updateDto.ImageFile, key);
                    
                    // Set the URL as the File property
                    updateDto.File = fileUrl;

                    // TODO: Consider deleting the old image file from S3
                }

                var result = await _propertyImageFacade.UpdatePropertyImageAsync(id, updateDto, cancellationToken);
                if (!result.Success)
                    return NotFound(result.Messages);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating image: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                // First, get the image to get its URL
                var image = await _propertyImageFacade.GetPropertyImageByIdAsync(id, cancellationToken);
                if (!image.Success)
                    return NotFound(image.Messages);

                // Extract the key from the URL - assumes URL structure from S3Service.GetFileUrl
                if (!string.IsNullOrEmpty(image.File) && image.File.Contains("amazonaws.com/"))
                {
                    string key = image.File.Split("amazonaws.com/").Last();
                    
                    // Delete the file from S3
                    await _s3Service.DeleteFileAsync(key);
                }

                // Delete the image record from the database
                var result = await _propertyImageFacade.DeletePropertyImageAsync(id, cancellationToken);
                if (!result.Success)
                    return NotFound(result.Messages);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting image: {ex.Message}");
            }
        }
    }
}
