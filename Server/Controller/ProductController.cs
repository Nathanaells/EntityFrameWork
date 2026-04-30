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

        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            ApiResponseDto<ProductResponseDTO> response = await _productService.CreateProduct(
                productDto,
                userId
            );

            if (!response.Success)
            {
                return StatusCode(400, response.Errors);
            }

            return Ok(response.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the product.", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            ApiResponseDto<ProductResponseDTO> response = await _productService.GetProductById(
                id,
                userId
            );

            if (!response.Success)
            {
                return NotFound(response.Errors);
            }

            return Ok(response.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the product.", error = ex.Message });
        }
    }

    [HttpGet("store/{storeId}")]
    public async Task<IActionResult> GetProductsByStoreId(int storeId)
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            ApiResponseDto<List<ProductResponseDTO>> response =
                await _productService.GetProductsByStoreId(storeId, userId);

            if (!response.Success)
            {
                return NotFound(response.Errors);
            }

            return Ok(response.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving products for the store.", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDto)
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            ApiResponseDto<ProductResponseDTO> response = await _productService.UpdateProduct(
                id,
                productDto,
                userId
            );

            if (!response.Success)
            {
                return NotFound(response.Errors);
            }

            return Ok(response.Data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the product.", error = ex.Message });
        }
    }


}
