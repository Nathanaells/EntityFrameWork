using Microsoft.EntityFrameworkCore;
using Server.Repository.Interfaces;

public class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _context;

    public StoreRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Store> GetStoreByIdAsync(int storeId)
    {
        Store store = await _context.Stores.FindAsync(storeId);

        return store;
    }

    public async Task<List<Store>> GetStoresByUserIdAsync(string userId)
    {
        List<Store> stores = await _context.Stores.Where(s => s.UserId == userId).ToListAsync();
        return stores;
    }

    public async Task<Store> GetStoreByIdWithUserAsync(int storeId)
    {
        Store? store = await _context
            .Stores.Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == storeId);

        return store;
    }

    public async Task<Store> CreateStoreAsync(Store store)
    {
        _context.Stores.Add(store);
        await _context.SaveChangesAsync();

        await _context.Entry(store).ReloadAsync();
        return store;
    }

    public async Task<Store> UpdateStoreAsync(Store store)
    {
        _context.Stores.Update(store);
        await _context.SaveChangesAsync();

        await _context.Entry(store).ReloadAsync();
        return store;
    }

    public async Task<bool> DeleteStoreAsync(int storeId)
    {
        Store? store = await _context.Stores.FindAsync(storeId);

        if (store == null)
        {
            return false;
        }

        _context.Stores.Remove(store);
        await _context.SaveChangesAsync();
        return true;
    }
}
