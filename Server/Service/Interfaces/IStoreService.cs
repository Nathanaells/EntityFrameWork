namespace Implemented_MVC.Service.Interfaces;

using Implemented_MVC.DTOs;

public interface IStoreService
{
    public Task<ServiceResult<StoreResponseDTO>> CreateStore(StoreDTO storeDto, string userId);

    public Task<ServiceResult<StoreResponseDTO>> GetStoreById(int id, string userId);
    public Task<ServiceResult<List<StoreResponseDTO>>> GetStoresByUserId(string userId);
    public Task<ServiceResult<StoreResponseDTO>> UpdateStore(int id, UpdateStoreDTO req, string userId);
    public Task<ServiceResult<bool>> DeleteStore(int id, string userId);
}
