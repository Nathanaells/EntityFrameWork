using Implemented_MVC.DTOs;
using Implemented_MVC.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        ServiceResult<RegisterResponseDTO> result = await _authService.RegisterAsync(registerDto);

        if (!result.Success)
        {
            return BadRequest(
                new
                {
                    status = false,
                    message = result.Message,
                    error = result.Errors,
                }
            );
        }

        return StatusCode(
            201,
            new
            {
                status = true,
                message = result.Message,
                data = result.Data,
            }
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        ServiceResult<LoginResponseDTO> result = await _authService.LoginAsync(loginDto);

        if (!result.Success)
        {
            return Unauthorized(
                new
                {
                    status = false,
                    message = result.Message,
                    error = result.Errors,
                }
            );
        }

        return Ok(
            new
            {
                status = true,
                message = result.Message,
                data = result.Data,
            }
        );
    }
}
