using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Implemented_MVC.DTOs;
using Implemented_MVC.Service.Interfaces;
using Server.Repository.Interfaces;

public class ProductService : IProductService
{
    private readonly IValidator<ProductCreateDTO> _productValidator;
    private readonly IValidator<UpdateProductDTO> _productUpdateValidator;

    private readonly IProductRepository _productRepository;
    private readonly IStoreRepository _storeRepository;

    private readonly IMapper _mapper;

    public ProductService(
        IValidator<ProductCreateDTO> productValidator,
        IValidator<UpdateProductDTO> productUpdateValidator,
        IProductRepository productRepository,
        IStoreRepository storeRepository,
        IMapper mapper
    )
    {
        _productValidator = productValidator;
        _productUpdateValidator = productUpdateValidator;
        _productRepository = productRepository;
        _storeRepository = storeRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto<ProductResponseDTO>> CreateProduct(
        ProductCreateDTO productDto,
        string userId,
        int storeId
    )
    {
        ValidationResult validationResult = _productValidator.Validate(productDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "Invalid product data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        Store? store = await _storeRepository.GetStoreByIdAsync(storeId);

        if (store == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "Store not found.",
                new List<string> { "Store with the provided ID does not exist." }
            );
        }

        if (store.UserId != userId)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "Access denied.",
                new List<string> { "You are not the owner of this store." }
            );
        }

        Product newProduct = _mapper.Map<Product>(productDto);
        newProduct.StoreId = storeId;
        newProduct.Store = store;

        await _productRepository.CreateProductAsync(newProduct);

        ProductResponseDTO response = _mapper.Map<ProductResponseDTO>(newProduct);

        return ApiResponseDto<ProductResponseDTO>.SuccessResult(
            response,
            "Product created successfully."
        );
    }

    public async Task<ApiResponseDto<ProductResponseDTO>> GetProductById(ProductRequestDTO request)
    {
        Product? product = await _productRepository.GetProductByIdWithStoreAsync(request.Id);

        if (product == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "Product not found.",
                new List<string> { "Product with the provided ID does not exist." }
            );
        }

        if (product.Store == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "Store not found.",
                new List<string> { "Store with the provided ID does not exist." }
            );
        }

        if (product.Store.UserId != request.UserId)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "Access denied.",
                new List<string> { "You are not the owner of this store." }
            );
        }

        ProductResponseDTO response = _mapper.Map<ProductResponseDTO>(product);

        return ApiResponseDto<ProductResponseDTO>.SuccessResult(
            response,
            "Success retrieving product."
        );
    }

    public async Task<ApiResponseDto<List<ProductResponseDTO>>> GetProductsByStoreId(
        ProductRequestDTO request
    )
    {
        Store? store = await _storeRepository.GetStoreByIdAsync(request.StoreId);

        if (store == null)
        {
            return ApiResponseDto<List<ProductResponseDTO>>.ErrorResult("Store not found.");
        }

        if (store.UserId != request.UserId)
        {
            return ApiResponseDto<List<ProductResponseDTO>>.ErrorResult("Access denied.");
        }

        List<Product> products = await _productRepository.GetProductByStoreIdAsync(request.StoreId);

        List<ProductResponseDTO> productResponses = _mapper
            .Map<List<ProductResponseDTO>>(products)
            .ToList();

        return ApiResponseDto<List<ProductResponseDTO>>.SuccessResult(productResponses);
    }

    public async Task<ApiResponseDto<ProductResponseDTO>> UpdateProduct(UpdateProductRequest req)
    {
        ValidationResult validationResult = _productUpdateValidator.Validate(req.Data);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult(
                "Invalid product data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        Product? existingProduct = await _productRepository.GetProductByIdWithStoreAsync(req.Id);

        if (existingProduct == null)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Product not found.");
        }

        if (existingProduct.Store == null || existingProduct.Store.UserId != req.UserId)
        {
            return ApiResponseDto<ProductResponseDTO>.ErrorResult("Access denied.");
        }

        _mapper.Map(req.Data, existingProduct);

        await _productRepository.UpdateProductAsync(existingProduct);

        ProductResponseDTO response = _mapper.Map<ProductResponseDTO>(existingProduct);

        return ApiResponseDto<ProductResponseDTO>.SuccessResult(
            response,
            "Product updated successfully."
        );
    }

    public async Task<ApiResponseDto<bool>> DeleteProduct(ProductRequestDTO request)
    {
        Product? productWithStore = await _productRepository.GetProductByIdWithStoreAsync(
            request.Id
        );

        if (productWithStore == null)
        {
            return ApiResponseDto<bool>.ErrorResult("Product not found.");
        }

        if (productWithStore.Store == null || productWithStore.Store.UserId != request.UserId)
        {
            return ApiResponseDto<bool>.ErrorResult("Access denied.");
        }
        await _productRepository.DeleteProductAsync(productWithStore.Id);

        return ApiResponseDto<bool>.SuccessResult(true);
    }
}
