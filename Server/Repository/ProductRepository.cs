using Microsoft.EntityFrameworkCore;
using Server.Repository.Interfaces;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        Product? product = await _context.Products.FindAsync(productId);
        return product;
    }

    public async Task<List<Product>> GetProductByStoreIdAsync(int storeId)
    {
        List<Product> products = await _context
            .Products.Include(p => p.Store)
            .Where(p => p.StoreId == storeId)
            .ToListAsync();

        return products;
    }

    public async Task<Product> GetProductByIdWithStoreAsync(int productId)
    {
        Product? product = await _context
            .Products.Include(p => p.Store)
            .FirstOrDefaultAsync(p => p.Id == productId);

        return product;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        List<Product> products = await _context.Products.OrderBy(p => p.Name).ToListAsync();

        return products;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        await _context.Entry(product).Reference(p => p.Store).LoadAsync();
        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();

        await _context.Entry(product).Reference(p => p.Store).LoadAsync();
        return product;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        Product? product = await GetProductByIdAsync(productId);

        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();
        return true;
    }
}
