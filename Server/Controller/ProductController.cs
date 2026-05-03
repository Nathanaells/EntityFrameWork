using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Implemented_MVC.DTOs;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO productDto)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { status = false, message = "Unauthorized", error = "Unauthorized" });
        }

        ApiResponseDto<ProductResponseDTO> response = await _productService.CreateProduct(
            productDto,
            userId
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { status = false, message = "Unauthorized", error = "Unauthorized" });
        }

        ApiResponseDto<ProductResponseDTO> response = await _productService.GetProductById(
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
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDto)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized(new { status = false, message = "Unauthorized", error = "Unauthorized" });
        }

        ApiResponseDto<ProductResponseDTO> response = await _productService.UpdateProduct(
            id,
            productDto,
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
    public async Task<IActionResult> DeleteProduct(int id)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return StatusCode(403, new { status = false, message = "Forbidden", error = "Forbidden" });
        }

        ApiResponseDto<bool> response = await _productService.DeleteProduct(id, userId);

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
