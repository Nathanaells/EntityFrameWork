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
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { status = false, message = "Unauthorized", error = "Unauthorized" });
        }

        ApiResponseDto<StoreResponseDTO> response = await _storeService.CreateStore(
            storeDto,
            userId
        );

        if (!response.Success)
        {
            return BadRequest(
                new
                {
                    status = false,
                    message = response.Message,
                    error = response.Errors
                }
            );
        }

        return StatusCode(
            201,
            new
            {
                status = true,
                message = response.Message,
                data = response.Data
            }
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStoreById(int id)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { status = false, message = "Unauthorized", error = "Unauthorized" });
        }

        ApiResponseDto<StoreResponseDTO> response = await _storeService.GetStoreById(
            id,
            userId
        );

        if (!response.Success)
        {
            return NotFound(
                new
                {
                    status = false,
                    message = response.Message,
                    error = response.Errors
                }
            );
        }

        return Ok(
            new
            {
                status = true,
                message = response.Message,
                data = response.Data
            }
        );
    }



    [HttpGet("stores")]
    public async Task<IActionResult> GetAllStores()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { status = false, message = "Unauthorized", error = "Unauthorized" });
        }

        ApiResponseDto<List<StoreResponseDTO>> response = await _storeService.GetStoresByUserId(
            userId
        );

        if (!response.Success)
        {
            return NotFound(
                new
                {
                    status = false,
                    message = response.Message,
                    error = response.Errors
                }
            );
        }

        return Ok(
            new
            {
                status = true,
                message = response.Message,
                data = response.Data
            }
        );
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStore(int id)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { status = false, message = "Unauthorized", error = "Unauthorized" });
        }

        ApiResponseDto<bool> response = await _storeService.DeleteStore(id, userId);

        if (!response.Success)
        {
            return NotFound(
                new
                {
                    status = false,
                    message = response.Message,
                    error = response.Errors
                }
            );
        }

        return Ok(
            new
            {
                status = true,
                message = "Store deleted successfully.",
                data = response.Data
            }
        );
    }
}
