namespace Server.Repository.Interfaces;

public interface IProductRepository
{
    public Task<Product> GetProductByIdAsync(int productId);
    public Task<List<Product>> GetProductByStoreIdAsync(int storeId);
    public Task<Product> GetProductByIdWithStoreAsync(int productId);
    public Task<List<Product>> GetAllAsync();
    public Task<Product> CreateProductAsync(Product product);
    public Task<Product> UpdateProductAsync(Product product);

    public Task<bool> DeleteProductAsync(int productId);
}
