using System.Security.Claims;
using Implemented_MVC.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        ApiResponseDto<User> response = await _userService.GetCurrentUserAsync(userId);

        if (!response.Success)
        {
            return NotFound(response.Errors);
        }

        return Ok(response.Data);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserDTO updateUserDto)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        ApiResponseDto<User> response = await _userService.UpdateCurrentUserAsync(
            userId,
            updateUserDto
        );

        if (!response.Success)
        {
            return BadRequest(response.Errors);
        }

        return Ok(response.Data);
    }
}
