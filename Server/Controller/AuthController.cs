using Implemented_MVC.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Service;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        ApiResponseDto<RegisterResponseDTO> result = await _authService.RegisterAsync(
            registerDto
        );

        if (!result.Success)
        {
            return BadRequest(
                new
                {
                    status = false,
                    message = result.Message,
                    error = result.Errors
                }
            );
        }

        return StatusCode(
            201,
            new
            {
                status = true,
                message = result.Message,
                data = result.Data
            }
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        ApiResponseDto<LoginResponseDTO> result = await _authService.LoginAsync(loginDto);

        if (!result.Success)
        {
            return Unauthorized(
                new
                {
                    status = false,
                    message = result.Message,
                    error = result.Errors
                }
            );
        }

        return Ok(
            new
            {
                status = true,
                message = result.Message,
                data = result.Data
            }
        );
    }
}
