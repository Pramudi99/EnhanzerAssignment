using EnhanzerAssignment.API.DTOs;
using EnhanzerAssignment.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnhanzerAssignment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IExternalAuthService _externalAuthService;

        public AuthController(
            IExternalAuthService externalAuthService)
        {
            _externalAuthService = externalAuthService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new
                {
                    message = "Email is required."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    message = "Password is required."
                });
            }

            var result =
                await _externalAuthService.LoginAsync(request);

            if (!result.Success)
            {
                return Unauthorized(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message,
                locations = result.Locations
            });
        }
    }
}