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
        public async Task<ActionResult<IEnumerable<OwnerDto>>> GetAll(CancellationToken cancellationToken)
        {
            var owners = await _ownerFacade.GetAllOwnersAsync(cancellationToken);
            if (owners == null || !owners.Any())
                return NotFound("No owners found.");    

            return Ok(owners);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerDto>> GetById(string id, CancellationToken cancellationToken)
        {
            var owner = await _ownerFacade.GetOwnerByIdAsync(id, cancellationToken);
            if (!owner.Success)
                return NotFound(owner.Messages);

            return Ok(owner);
        }

        [HttpPost]
        public async Task<ActionResult<OwnerDto>> Create([FromBody] CreateOwnerDto createDto, CancellationToken cancellationToken)
        {
            var owner = await _ownerFacade.CreateOwnerAsync(createDto, cancellationToken);
            if (!owner.Success)
                return BadRequest(owner.Messages);

            return CreatedAtAction(nameof(GetById), new { id = owner.Id }, owner);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateOwnerDto updateDto, CancellationToken cancellationToken)
        {
            var owner = await _ownerFacade.UpdateOwnerAsync(id, updateDto, cancellationToken);
            if (!owner.Success)
                return BadRequest(owner.Messages);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var owner = await _ownerFacade.DeleteOwnerAsync(id, cancellationToken);
            if (!owner.Success)
                return BadRequest(owner.Messages);

            return NoContent();
        }
    }
}
