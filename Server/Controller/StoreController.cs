using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Implemented_MVC.DTOs;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StoreController : ControllerBase
{
    private readonly StoreService _storeService;

    public StoreController(StoreService storeService)
    {
        _storeService = storeService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStore([FromBody] StoreDTO storeDto)
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Console.WriteLine($"User ID from token: {userId}");

            if (string.IsNullOrWhiteSpace(userId))
            {
                return StatusCode(401, new { message = "Unauthorized: User ID not found in token." });
            }

            ApiResponseDto<StoreResponseDTO> response = await _storeService.CreateStore(
                storeDto,
                userId
            );

            if (!response.Success)
            {
                return BadRequest(new { message = response.Message });
            }

            return StatusCode(201, response.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the store.", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStoreById(int id)
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            ApiResponseDto<StoreResponseDTO> response = await _storeService.GetStoreById(
                id,
                userId
            );

            if (!response.Success)
            {
                return NotFound(new { message = response.Message });
            }

            return Ok(response.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the store.", error = ex.Message });
        }
    }

    [HttpGet("stores")]
    public async Task<IActionResult> GetAllStores()
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            ApiResponseDto<List<StoreResponseDTO>> response = await _storeService.GetStoresByUserId(
                userId
            );

            if (!response.Success)
            {
                return NotFound(new { message = response.Message });
            }

            return Ok(response.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the stores.", error = ex.Message });

        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStore(int id)
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            ApiResponseDto<bool> response = await _storeService.DeleteStore(id, userId);

            if (!response.Success)
            {
                return NotFound(new { message = response.Message });
            }

            return Ok(new { deleted = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the store.", error = ex.Message });
        }
    }
}
