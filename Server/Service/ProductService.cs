using FluentValidation;
using FluentValidation.Results;
using Implemented_MVC.DTOs;
using Microsoft.EntityFrameworkCore;

public class ProductService
{
    private readonly IValidator<ProductCreateDTO> _productValidator;
    private readonly IValidator<UpdateProductDTO> _productUpdateValidator;

    private readonly AppDbContext _context;

    public ProductService(
        IValidator<ProductCreateDTO> productValidator,
        IValidator<UpdateProductDTO> productUpdateValidator,
        AppDbContext context
    )
    {
        _productValidator = productValidator;
        _productUpdateValidator = productUpdateValidator;
        _context = context;
    }

    public async Task<ApiResponseDto<ProductResponseDTO>> CreateProduct(ProductCreateDTO productDto, string userId)
    {
        ValidationResult validationResult = _productValidator.Validate(productDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "Invalid product data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        Store? store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == productDto.StoreId);

        if (store == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Store not found.");
        }

        if (store.UserId != userId)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Access denied.");
        }

        Product newProduct = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            StoreId = productDto.StoreId,
        };

        await _context.Products.AddAsync(newProduct);

        await _context.SaveChangesAsync();

        return ApiResponseDto<ProductResponseDTO>.SuccessResult(MapProductResponse(newProduct));
    }

    public async Task<ApiResponseDto<ProductResponseDTO>> GetProductById(int id, string userId)
    {
        Product? product = await _context
            .Products.Include(p => p.Store)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (product == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Product not found.");
        }

        if (product.Store == null || product.Store.UserId != userId)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Access denied.");
        }

        return ApiResponseDto<ProductResponseDTO>.SuccessResult(MapProductResponse(product));
    }

    public async Task<ApiResponseDto<List<ProductResponseDTO>>> GetProductsByStoreId(int storeId, string userId)
    {
        Store? store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == storeId);

        if (store == null)
        {
            return ApiResponseDto<List<ProductResponseDTO>>.ErrorResult("Store not found.");
        }

        if (store.UserId != userId)
        {
            return ApiResponseDto<List<ProductResponseDTO>>.ErrorResult("Access denied.");
        }

        List<Product> products = await _context
            .Products.Where(p => p.StoreId == storeId)
            .ToListAsync();

        List<ProductResponseDTO> productResponses = products
            .Select(MapProductResponse)
            .ToList();

        return ApiResponseDto<List<ProductResponseDTO>>.SuccessResult(productResponses);
    }

    public async Task<ApiResponseDto<ProductResponseDTO>> UpdateProduct(
        int id,
        UpdateProductDTO productDto,
        string userId
    )
    {
        ValidationResult validationResult = _productUpdateValidator.Validate(productDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "Invalid product data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        if (string.IsNullOrWhiteSpace(productDto.Name) && productDto.Price == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "No data to update.",
                new List<string> { "Provide at least one field to update." }
            );
        }

        Product? existingProduct = await _context
            .Products.Include(p => p.Store)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (existingProduct == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Product not found.");
        }

        if (existingProduct.Store == null || existingProduct.Store.UserId != userId)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Access denied.");
        }

        if (!string.IsNullOrWhiteSpace(productDto.Name))
        {
            existingProduct.Name = productDto.Name;
        }

        if (productDto.Price.HasValue)
        {
            existingProduct.Price = productDto.Price.Value;
        }

        await _context.SaveChangesAsync();

        return ApiResponseDto<ProductResponseDTO>.SuccessResult(
            MapProductResponse(existingProduct)
        );
    }

    public async Task<ApiResponseDto<bool>> DeleteProduct(int id, string userId)
    {
        Product? deletedProduct = await _context
            .Products.Include(p => p.Store)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (deletedProduct == null)
        {
            return ApiResponseDto<bool>.ErrorResult("Product not found.");
        }

        if (deletedProduct.Store == null || deletedProduct.Store.UserId != userId)
        {
            return ApiResponseDto<bool>.ErrorResult("Access denied.");
        }
        _context.Products.Remove(deletedProduct);

        await _context.SaveChangesAsync();
        return ApiResponseDto<bool>.SuccessResult(true);
    }

    private static ProductResponseDTO MapProductResponse(Product product)
    {
        return new ProductResponseDTO
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            StoreId = product.StoreId,
        };
    }
}
