namespace Implemented_MVC.Service.Interfaces;

using Implemented_MVC.DTOs;

public interface IProductService
{
    public Task<ApiResponseDto<ProductResponseDTO>> CreateProduct(
        ProductCreateDTO productDto,
        string userId,
        int storeId
    );
    public Task<ApiResponseDto<ProductResponseDTO>> GetProductById(ProductRequestDTO request);
    public Task<ApiResponseDto<List<ProductResponseDTO>>> GetProductsByStoreId(
        ProductRequestDTO request
    );

    public Task<ApiResponseDto<ProductResponseDTO>> UpdateProduct(UpdateProductRequest req);
    public Task<ApiResponseDto<bool>> DeleteProduct(ProductRequestDTO request);
}
