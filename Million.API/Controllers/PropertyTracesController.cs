using Microsoft.AspNetCore.Mvc;
using Million.Application.PropertyTraces;
using Million.Domain.DTOs;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyTracesController : ControllerBase
    {
        private readonly PropertyTraceFacade _propertyTraceFacade;

        public PropertyTracesController(PropertyTraceFacade propertyTraceFacade)
        {
            _propertyTraceFacade = propertyTraceFacade;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyTraceDto>> GetById(string id, CancellationToken cancellationToken)
        {
            var trace = await _propertyTraceFacade.GetPropertyTraceByIdAsync(id, cancellationToken);
            if (!trace.Success)
                return NotFound(trace.Messages);

            return Ok(trace);
        }

        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<PropertyTraceDto>>> GetByPropertyId(string propertyId, CancellationToken cancellationToken)
        {
            try
            {
                var traces = await _propertyTraceFacade.GetTracesByPropertyIdAsync(propertyId, cancellationToken);
                if (traces == null || !traces.Any())
                    return NotFound($"No traces found for property with ID {propertyId}.");

                return Ok(traces);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("daterange")]
        public async Task<ActionResult<IEnumerable<PropertyTraceDto>>> GetByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            CancellationToken cancellationToken)
        {
            try
            {
                if (startDate > endDate)
                    return BadRequest("Start date must be before or equal to end date.");

                var traces = await _propertyTraceFacade.GetTracesByDateRangeAsync(startDate, endDate, cancellationToken);
                if (traces == null || !traces.Any())
                    return NotFound($"No traces found between {startDate:d} and {endDate:d}.");

                return Ok(traces);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PropertyTraceDto>> Create([FromBody] CreatePropertyTraceDto createDto, CancellationToken cancellationToken)
        {
            try
            {
                var trace = await _propertyTraceFacade.CreatePropertyTraceAsync(createDto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = trace.IdPropertyTrace }, trace);
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
    }
}
