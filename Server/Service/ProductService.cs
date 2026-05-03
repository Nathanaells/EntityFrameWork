using FluentValidation;
using FluentValidation.Results;
using Implemented_MVC.DTOs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
public class ProductService
{
    private readonly IValidator<ProductCreateDTO> _productValidator;
    private readonly IValidator<UpdateProductDTO> _productUpdateValidator;

    private readonly IMapper _mapper;

    private readonly AppDbContext _context;

    public ProductService(
        IValidator<ProductCreateDTO> productValidator,
        IValidator<UpdateProductDTO> productUpdateValidator,
        AppDbContext context,
        IMapper mapper
    )
    {
        _productValidator = productValidator;
        _productUpdateValidator = productUpdateValidator;
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto<ProductResponseDTO>> CreateProduct(ProductCreateDTO productDto, string userId, int storeId)
    {
        ValidationResult validationResult = _productValidator.Validate(productDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "Invalid product data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        Store? store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == storeId);

        if (store == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Store not found.", new List<string> { "Store with the provided ID does not exist." });
        }

        if (store.UserId != userId)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Access denied.", new List<string> { "You are not the owner of this store." });
        }

        Product newProduct = _mapper.Map<Product>(productDto);
        newProduct.StoreId = storeId;
        newProduct.Store = store;

        await _context.Products.AddAsync(newProduct);
        await _context.SaveChangesAsync();

        ProductResponseDTO response = _mapper.Map<ProductResponseDTO>(newProduct);

        return ApiResponseDto<ProductResponseDTO>.SuccessResult(response, "Product created successfully.");
    }

    public async Task<ApiResponseDto<ProductResponseDTO>> GetProductById(int storeId, int id, string userId)
    {
        Product? product = await _context.Products.FirstOrDefaultAsync(i => i.Id == id);

        if (product == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Product not found.", new List<string> { "Product with the provided ID does not exist." });
        }



        Store? store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == storeId);

        if (store == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Store not found.", new List<string> { "Store with the provided ID does not exist." });
        }

        if (store.UserId != userId)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Access denied.", new List<string> { "You are not the owner of this store." });
        }

        ProductResponseDTO response = _mapper.Map<ProductResponseDTO>(product);


        return ApiResponseDto<ProductResponseDTO>.SuccessResult(response, "Success retrieving product.");
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

        List<Product> products = await _context.Products.Where(p => p.StoreId == storeId).ToListAsync();
        List<ProductResponseDTO> productResponses = _mapper.Map<List<ProductResponseDTO>>(products).ToList();

        return ApiResponseDto<List<ProductResponseDTO>>.SuccessResult(productResponses);
    }

    public async Task<ApiResponseDto<ProductResponseDTO>> UpdateProduct(
     UpdateProductRequest req
    )
    {
        ValidationResult validationResult = _productUpdateValidator.Validate(req.Data);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "Invalid product data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }


        Product? existingProduct = await _context
            .Products.Include(p => p.Store)
            .FirstOrDefaultAsync(i => i.Id == req.ProductId);

        if (existingProduct == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Product not found.");
        }

        if (existingProduct.Store == null || existingProduct.Store.UserId != req.UserId)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Access denied.");
        }


        _mapper.Map(req.Data, existingProduct);
        await _context.SaveChangesAsync();

        ProductResponseDTO response = _mapper.Map<ProductResponseDTO>(existingProduct);

        return ApiResponseDto<ProductResponseDTO>.SuccessResult(response, "Product updated successfully.");
    }

    public async Task<ApiResponseDto<bool>> DeleteProduct(int id, string userId, int storeId)
    {
        Product? deletedProduct = await _context
            .Products
            .FirstOrDefaultAsync(p => p.Id == id);

        Store? store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == storeId);

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


}
