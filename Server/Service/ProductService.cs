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

    public async Task<ServiceResult<ProductResponseDTO>> CreateProduct(
        ProductCreateDTO productDto,
        string userId,
        int storeId
    )
    {
        ValidationResult validationResult = _productValidator.Validate(productDto);

        if (!validationResult.IsValid)
        {
            return ServiceResult<ProductResponseDTO>.ErrorResult(
                "Invalid product data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        Store? store = await _storeRepository.GetStoreByIdAsync(storeId);

        if (store == null)
        {
            return ServiceResult<ProductResponseDTO>.ErrorResult(
                "Store not found.",
                new List<string> { "Store with the provided ID does not exist." }
            );
        }

        if (store.UserId != userId)
        {
            return ServiceResult<ProductResponseDTO>.ErrorResult(
                "Access denied.",
                new List<string> { "You are not the owner of this store." }
            );
        }

        Product newProduct = _mapper.Map<Product>(productDto);
        newProduct.StoreId = storeId;
        newProduct.Store = store;

        await _productRepository.CreateProductAsync(newProduct);

        ProductResponseDTO response = _mapper.Map<ProductResponseDTO>(newProduct);

        return ServiceResult<ProductResponseDTO>.SuccessResult(
            response,
            "Product created successfully."
        );
    }

    public async Task<ServiceResult<ProductResponseDTO>> GetProductById(ProductRequestDTO request)
    {
        Product? product = await _productRepository.GetProductByIdWithStoreAsync(request.Id);

        if (product == null)
        {
            return ServiceResult<ProductResponseDTO>.ErrorResult(
                "Product not found.",
                new List<string> { "Product with the provided ID does not exist." }
            );
        }

        if (product.Store == null)
        {
            return ServiceResult<ProductResponseDTO>.ErrorResult(
                "Store not found.",
                new List<string> { "Store with the provided ID does not exist." }
            );
        }

        if (product.Store.UserId != request.UserId)
        {
            return ServiceResult<ProductResponseDTO>.ErrorResult(
                "Access denied.",
                new List<string> { "You are not the owner of this store." }
            );
        }

        ProductResponseDTO response = _mapper.Map<ProductResponseDTO>(product);

        return ServiceResult<ProductResponseDTO>.SuccessResult(
            response,
            "Success retrieving product."
        );
    }

    public async Task<ServiceResult<List<ProductResponseDTO>>> GetProductsByStoreId(
        ProductRequestDTO request
    )
    {
        Store? store = await _storeRepository.GetStoreByIdAsync(request.StoreId);

        if (store == null)
        {
            return ServiceResult<List<ProductResponseDTO>>.ErrorResult(
                "Store not found.",
                new List<string> { "Store with the provided ID does not exist." }
            );
        }

        if (store.UserId != request.UserId)
        {
            return ServiceResult<List<ProductResponseDTO>>.ErrorResult(
                "Access denied.",
                new List<string> { "You are not the owner of this store." }
            );
        }

        List<Product> products = await _productRepository.GetProductByStoreIdAsync(request.StoreId);

        List<ProductResponseDTO> productResponses = _mapper
            .Map<List<ProductResponseDTO>>(products)
            .ToList();

        return ServiceResult<List<ProductResponseDTO>>.SuccessResult(productResponses);
    }

    public async Task<ServiceResult<ProductResponseDTO>> UpdateProduct(UpdateProductRequest req)
    {
        ValidationResult validationResult = _productUpdateValidator.Validate(req.Data);

        if (!validationResult.IsValid)
        {
            return ServiceResult<ProductResponseDTO>.ErrorResult(
                "Invalid product data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        Product? existingProduct = await _productRepository.GetProductByIdWithStoreAsync(req.Id);

        if (existingProduct == null)
        {
            return ServiceResult<ProductResponseDTO>.ErrorResult(
                "Product not found.",
                new List<string> { "Product with the provided ID does not exist." }
            );
        }

        if (existingProduct.Store == null || existingProduct.Store.UserId != req.UserId)
        {
            return ServiceResult<ProductResponseDTO>.ErrorResult(
                "Access denied.",
                new List<string> { "You are not the owner of this product." }
            );
        }

        _mapper.Map(req.Data, existingProduct);

        await _productRepository.UpdateProductAsync(existingProduct);

        ProductResponseDTO response = _mapper.Map<ProductResponseDTO>(existingProduct);

        return ServiceResult<ProductResponseDTO>.SuccessResult(
            response,
            "Product updated successfully."
        );
    }

    public async Task<ServiceResult<bool>> DeleteProduct(ProductRequestDTO request)
    {
        Product? productWithStore = await _productRepository.GetProductByIdWithStoreAsync(
            request.Id
        );

        if (productWithStore == null)
        {
            return ServiceResult<bool>.ErrorResult(
                "Product not found.",
                new List<string> { "Product with the provided ID does not exist." }
            );
        }

        if (productWithStore.Store == null || productWithStore.Store.UserId != request.UserId)
        {
            return ServiceResult<bool>.ErrorResult(
                "Access denied.",
                new List<string> { "You are not the owner of this product." }
            );
        }
        await _productRepository.DeleteProductAsync(productWithStore.Id);

        return ServiceResult<bool>.SuccessResult(true);
    }
}
