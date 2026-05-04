using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Implemented_MVC.DTOs;

[ApiController]
[Route("api/store/{storeId}/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO productDto, [FromRoute] int storeId)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { status = false, message = "Unauthorized", error = "Unauthorized" });
        }

        ApiResponseDto<ProductResponseDTO> response = await _productService.CreateProduct(
            productDto,
            userId,
            storeId
        );

        if (!response.Success)
        {
            return StatusCode(
                400,
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

    [HttpGet]
    public async Task<IActionResult> GetProductsByStoreId([FromRoute] int storeId)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { status = false, message = "Unauthorized", error = "Unauthorized" });
        }

        ApiResponseDto<List<ProductResponseDTO>> response = await _productService.GetProductsByStoreId(storeId, userId);

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] int storeId, int id)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { status = false, message = "Unauthorized", error = "Unauthorized" });
        }

        ApiResponseDto<ProductResponseDTO> response = await _productService.GetProductById(
            storeId,
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



    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] int storeId, [FromBody] UpdateProductDTO productDto, int id)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { status = false, message = "Unauthorized", error = "Unauthorized" });
        }

        UpdateProductRequest updateRequest = new UpdateProductRequest
        {
            ProductId = id,
            StoreId = storeId,
            UserId = userId,
            Data = productDto
        };

        ApiResponseDto<ProductResponseDTO> response = await _productService.UpdateProduct(
            updateRequest
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
    public async Task<IActionResult> DeleteProduct([FromRoute] int storeId, int id)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return StatusCode(403, new { status = false, message = "Forbidden", error = "Forbidden" });
        }

        ApiResponseDto<bool> response = await _productService.DeleteProduct(id, userId, storeId);

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
                message = "Product deleted successfully.",
                data = response.Data
            }
        );
    }

}
