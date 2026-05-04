namespace Implemented_MVC.Service.Interfaces;

using Implemented_MVC.DTOs;

public interface IStoreService
{
    public Task<ApiResponseDto<StoreResponseDTO>> CreateStore(StoreDTO storeDto, string userId);

    public Task<ApiResponseDto<StoreResponseDTO>> GetStoreById(int id, string userId);
    public Task<ApiResponseDto<List<StoreResponseDTO>>> GetStoresByUserId(string userId);
    public Task<ApiResponseDto<StoreResponseDTO>> UpdateStore(UpdateStoreDTO req, string userId);
    public Task<ApiResponseDto<bool>> DeleteStore(int id, string userId);
}
