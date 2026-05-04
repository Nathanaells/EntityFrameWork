namespace Server.Repository.Interfaces;

public interface IStoreRepository
{
    public Task<Store> GetStoreByIdAsync(int storeId);
    public Task<List<Store>> GetStoresByUserIdAsync(string userId);
    public Task<Store> GetStoreByIdWithUserAsync(int storeId);
    public Task<Store> CreateStoreAsync(Store store);
    public Task<Store> UpdateStoreAsync(Store store);
    public Task<bool> DeleteStoreAsync(int storeId);
}
