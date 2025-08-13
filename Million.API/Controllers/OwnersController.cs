using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Million.Application.Owners;
using Million.Application.Services;
using Million.Domain.DTOs;
using System.IO;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnersController : ControllerBase
    {
        private readonly OwnerFacade _ownerFacade;
        private readonly IS3Service _s3Service;

        public OwnersController(OwnerFacade ownerFacade, IS3Service s3Service)
        {
            _ownerFacade = ownerFacade;
            _s3Service = s3Service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetAll([FromQuery] string? name, CancellationToken cancellationToken)
        {
            IEnumerable<OwnerDto> owners = string.IsNullOrWhiteSpace(name) 
                ? await _ownerFacade.GetAllOwnersAsync(cancellationToken)
                : await _ownerFacade.GetOwnersByNameContainingAsync(name, cancellationToken);

            return Ok(owners);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<OwnerDto>> GetById(string id, CancellationToken cancellationToken)
        {
            var owner = await _ownerFacade.GetOwnerByIdAsync(id, cancellationToken);
            if (!owner.Success)
                return NotFound(owner.Messages);

            return Ok(owner);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<OwnerDto>> Create([FromForm] CreateOwnerDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                if (createDto.PhotoFile != null)
                {
                    // Generate a unique filename to avoid conflicts
                    string fileExtension = Path.GetExtension(createDto.PhotoFile.FileName);
                    string fileName = $"{Guid.NewGuid()}{fileExtension}";
                    string key = $"owners/{fileName}";

                    // Upload the file to S3
                    string fileUrl = await _s3Service.UploadFileAsync(createDto.PhotoFile, key);
                    
                    // Set the URL as the Photo property
                    createDto.Photo = fileUrl;
                }

                var owner = await _ownerFacade.CreateOwnerAsync(createDto, cancellationToken);
                if (!owner.Success)
                    return BadRequest(owner.Messages);

                return CreatedAtAction(nameof(GetById), new { id = owner.Id }, owner);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading image: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> Update(string id, [FromForm] UpdateOwnerDto updateDto, CancellationToken cancellationToken)
        {
            try
            {
                // Get the existing owner to know the current photo URL
                var existingOwner = await _ownerFacade.GetOwnerByIdAsync(id, cancellationToken);
                if (!existingOwner.Success)
                    return NotFound(existingOwner.Messages);

                if (updateDto.PhotoFile != null)
                {
                    // Generate a unique filename to avoid conflicts
                    string fileExtension = Path.GetExtension(updateDto.PhotoFile.FileName);
                    string fileName = $"{Guid.NewGuid()}{fileExtension}";
                    string key = $"owners/{fileName}";

                    // Upload the file to S3
                    string fileUrl = await _s3Service.UploadFileAsync(updateDto.PhotoFile, key);
                    
                    // Set the URL as the Photo property
                    updateDto.Photo = fileUrl;

                    // If there was an existing photo, we could delete it here
                    if (!string.IsNullOrEmpty(existingOwner.Photo) && existingOwner.Photo.Contains("amazonaws.com/"))
                    {
                        try
                        {
                            string oldKey = existingOwner.Photo.Split("amazonaws.com/").Last();
                            await _s3Service.DeleteFileAsync(oldKey);
                        }
                        catch (Exception ex)
                        {
                            // Log the error but continue with the update
                            Console.WriteLine($"Failed to delete old image: {ex.Message}");
                        }
                    }
                }
                else if (updateDto.Photo == null)
                {
                    // If no new photo is provided and Photo is explicitly set to null, 
                    // it means the user wants to remove the photo
                    updateDto.Photo = null;

                    // Delete the existing photo if there was one
                    if (!string.IsNullOrEmpty(existingOwner.Photo) && existingOwner.Photo.Contains("amazonaws.com/"))
                    {
                        try
                        {
                            string oldKey = existingOwner.Photo.Split("amazonaws.com/").Last();
                            await _s3Service.DeleteFileAsync(oldKey);
                        }
                        catch (Exception ex)
                        {
                            // Log the error but continue with the update
                            Console.WriteLine($"Failed to delete old image: {ex.Message}");
                        }
                    }
                }

                var owner = await _ownerFacade.UpdateOwnerAsync(id, updateDto, cancellationToken);
                if (!owner.Success)
                    return BadRequest(owner.Messages);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating owner: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                // Get the owner to check if there's a photo to delete
                var existingOwner = await _ownerFacade.GetOwnerByIdAsync(id, cancellationToken);
                if (!existingOwner.Success)
                    return NotFound(existingOwner.Messages);

                // If there is a photo, delete it from S3
                if (!string.IsNullOrEmpty(existingOwner.Photo) && existingOwner.Photo.Contains("amazonaws.com/"))
                {
                    try
                    {
                        string key = existingOwner.Photo.Split("amazonaws.com/").Last();
                        await _s3Service.DeleteFileAsync(key);
                    }
                    catch (Exception ex)
                    {
                        // Log the error but continue with the deletion
                        Console.WriteLine($"Failed to delete image: {ex.Message}");
                    }
                }

                // Delete the owner from the database
                var owner = await _ownerFacade.DeleteOwnerAsync(id, cancellationToken);
                if (!owner.Success)
                    return BadRequest(owner.Messages);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting owner: {ex.Message}");
            }
        }
    }
}
