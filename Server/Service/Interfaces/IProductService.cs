namespace Implemented_MVC.Service.Interfaces;

using Implemented_MVC.DTOs;

public interface IProductService
{
    public Task<ServiceResult<ProductResponseDTO>> CreateProduct(
        ProductCreateDTO productDto,
        string userId,
        int storeId
    );
    public Task<ServiceResult<ProductResponseDTO>> GetProductById(ProductRequestDTO request);
    public Task<ServiceResult<List<ProductResponseDTO>>> GetProductsByStoreId(
        ProductRequestDTO request
    );

    public Task<ServiceResult<ProductResponseDTO>> UpdateProduct(UpdateProductRequest req);
    public Task<ServiceResult<bool>> DeleteProduct(ProductRequestDTO request);
}
