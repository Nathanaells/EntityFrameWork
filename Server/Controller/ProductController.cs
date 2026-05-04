using System.Security.Claims;
using Implemented_MVC.DTOs;
using Implemented_MVC.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/store/{storeId}/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromBody] ProductCreateDTO productDto,
        [FromRoute] int storeId
    )
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

    [HttpGet]
    public async Task<IActionResult> GetProductsByStoreId([FromRoute] int storeId)
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

        ProductRequestDTO request = new ProductRequestDTO { StoreId = storeId, UserId = userId };

        ApiResponseDto<List<ProductResponseDTO>> response =
            await _productService.GetProductsByStoreId(request);

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] int storeId, int id)
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

        ProductRequestDTO request = new ProductRequestDTO
        {
            StoreId = storeId,
            Id = id,
            UserId = userId,
        };

        ApiResponseDto<ProductResponseDTO> response = await _productService.GetProductById(request);

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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO productDto, int id)
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

        UpdateProductRequest updateRequest = new UpdateProductRequest
        {
            Id = id,
            UserId = userId,
            Data = productDto,
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int storeId, int id)
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return StatusCode(
                403,
                new
                {
                    status = false,
                    message = "Forbidden",
                    error = "Forbidden",
                }
            );
        }

        ProductRequestDTO request = new ProductRequestDTO
        {
            StoreId = storeId,
            Id = id,
            UserId = userId,
        };

        ApiResponseDto<bool> response = await _productService.DeleteProduct(request);

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
                message = "Product deleted successfully.",
                data = response.Data,
            }
        );
    }
}
