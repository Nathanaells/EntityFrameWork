using System.Security.Claims;
using Implemented_MVC.DTOs;
using Implemented_MVC.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(
                new
                {
                    status = false,
                    message = "You Must Login First",
                    error = "Unauthorized",
                }
            );
        }

        ApiResponseDto<UserResponseDTO> response = await _userService.GetCurrentUserAsync(userId);

        if (!response.Success)
        {
            return NotFound(
                new
                {
                    status = false,
                    message = response.Message,
                    error = response.Errors,
                }
            );
        }

        return Ok(
            new
            {
                status = true,
                message = response.Message,
                data = response.Data,
            }
        );
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserDTO updateUserDto)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(
                new
                {
                    status = false,
                    message = "Unauthorized",
                    error = "Unauthorized",
                }
            );
        }

        ApiResponseDto<UserResponseDTO> response = await _userService.UpdateCurrentUserAsync(
            userId,
            updateUserDto
        );

        if (!response.Success)
        {
            return BadRequest(
                new
                {
                    status = false,
                    message = response.Message,
                    error = response.Errors,
                }
            );
        }

        return Ok(
            new
            {
                status = true,
                message = response.Message,
                data = response.Data,
            }
        );
    }
}
