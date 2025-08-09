using Microsoft.AspNetCore.Mvc;
using Million.Application.Authentications;
using Million.Domain.DTOs;

namespace Million.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthentication _authentication;

        public AuthenticationController(IAuthentication authentication, ILogger<AuthenticationController> logger)
        {
            _authentication = authentication;
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest autorizacion, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login");
            _logger.LogInformation("User Name: {UserName}", autorizacion.UserName);
            LoginResponse Response = await _authentication.Login(autorizacion, cancellationToken);
            if (!Response.Success)
                return BadRequest(Response.Messages);

            return Ok(Response.Token);
        }
    }
}
