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
        try
        {
            ApiResponseDto<RegisterResponseDTO> result = await _authService.RegisterAsync(
                registerDto
            );

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return StatusCode(201, result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        try
        {
            ApiResponseDto<LoginResponseDTO> result = await _authService.LoginAsync(loginDto);

            if (!result.Success)
            {
                return Unauthorized();
            }

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
