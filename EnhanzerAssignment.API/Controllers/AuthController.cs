using EnhanzerAssignment.API.DTOs;
using EnhanzerAssignment.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnhanzerAssignment.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IExternalAuthService _externalAuthService;
        private readonly ILogger<AuthController> _logger;


        public AuthController(
            IExternalAuthService externalAuthService, JwtService jwtService, ILogger<AuthController> logger)
        {
            _externalAuthService = externalAuthService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpGet("test-version")]
        public IActionResult TestVersion()
        {
            return Ok(new
            {
                version = "logging-test-2026-09-13",
                message = "Latest code is running."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            _logger.LogWarning("========== LOGIN ENDPOINT CALLED ==========");

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

            // Generate JWT after successful authentication
            var token = _jwtService.GenerateToken(
            request.Email,
            result.UserCode);

            return Ok(new
            {
                message = result.Message,
                token = token,
                locations = result.Locations
            });
        }
    }
}