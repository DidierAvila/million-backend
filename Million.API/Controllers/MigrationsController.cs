using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Million.Infrastructure.Migrations;
using System.Threading;
using System.Threading.Tasks;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class MigrationsController : ControllerBase
    {
        private readonly MigrationService _migrationService;

        public MigrationsController(MigrationService migrationService)
        {
            _migrationService = migrationService;
        }

        [HttpPost("up")]
        public async Task<IActionResult> MigrateUp(CancellationToken cancellationToken)
        {
            var result = await _migrationService.MigrateUpAsync(cancellationToken);
            if (!result)
                return StatusCode(500, "Error al aplicar migraciones");

            return Ok("Migraciones aplicadas correctamente");
        }

        [HttpPost("down/{version:int}")]
        public async Task<IActionResult> MigrateDown(int version, CancellationToken cancellationToken)
        {
            var result = await _migrationService.MigrateDownToVersionAsync(version, cancellationToken);
            if (!result)
                return StatusCode(500, "Error al revertir migraciones");

            return Ok($"Migraciones revertidas correctamente hasta la versión {version}");
        }
    }
}
