using FluentValidation;
using FluentValidation.Results;
using Implemented_MVC.DTOs;
using Microsoft.EntityFrameworkCore;

public class ProductService
{
    private readonly IValidator<ProductDTO> _productValidator;

    private readonly AppDbContext _context;

    public ProductService(IValidator<ProductDTO> productValidator)
    {
        _productValidator = productValidator;
    }

    public async Task<ApiResponseDto<Product>> CreateProduct(ProductDTO productDto)
    {
        ValidationResult validationResult = _productValidator.Validate(productDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<Product>.ErrorResult(
                "Invalid product data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        Product newProduct = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            StoreId = productDto.StoreId,
        };

        await _context.Products.AddAsync(newProduct);

        await _context.SaveChangesAsync();

        return ApiResponseDto<Product>.SuccessResult(newProduct);
    }

    public async Task<ApiResponseDto<Product>> GetProductById(int id)
    {
        Product? product = await _context.Products.FirstOrDefaultAsync(i => i.Id == id);

        if (product == null)
        {
            return ApiResponseDto<Product>.ErrorResult("Product not found.");
        }

        return ApiResponseDto<Product>.SuccessResult(product);
    }

    public async Task<ApiResponseDto<List<Product>>> GetProductsByStoreId(int storeId)
    {
        // Store? product = await _context.Stores.FindAsync(storeId).Include(s => s.Products).FirstOrDefaultAsync();
        List<Product> products = await _context
            .Products.Where(p => p.StoreId == storeId)
            .ToListAsync();

        return ApiResponseDto<List<Product>>.SuccessResult(products);
    }

    public async Task<ApiResponseDto<Product>> UpdateProduct(int id, ProductDTO productDto)
    {
        ValidationResult validationResult = _productValidator.Validate(productDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<Product>.ErrorResult(
                "Invalid product data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        Product? existingProduct = await _context.Products.FirstOrDefaultAsync(i => i.Id == id);

        if (existingProduct == null)
        {
            return ApiResponseDto<Product>.ErrorResult("Product not found.");
        }

        existingProduct.Name = productDto.Name;
        existingProduct.Price = productDto.Price;
        existingProduct.StoreId = productDto.StoreId;

        await _context.SaveChangesAsync();

        return ApiResponseDto<Product>.SuccessResult(existingProduct);
    }

    public async Task<ApiResponseDto<bool>> DeleteProduct(int id)
    {
        Product? deletedProduct = await _context
            .Products.Where(p => p.Id == id)
            .FirstOrDefaultAsync();

        if (deletedProduct == null)
        {
            return ApiResponseDto<bool>.ErrorResult("Product not found.");
        }
        _context.Products.Remove(deletedProduct);

        await _context.SaveChangesAsync();
        return ApiResponseDto<bool>.SuccessResult(true);
    }
}
