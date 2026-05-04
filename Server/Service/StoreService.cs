using FluentValidation;
using FluentValidation.Results;
using Implemented_MVC.DTOs;
using Microsoft.EntityFrameworkCore;

public class StoreService
{
    private readonly AppDbContext _context;
    private readonly IValidator<StoreDTO> _storeValidator;

    public StoreService(AppDbContext context, IValidator<StoreDTO> storeValidator)
    {
        _context = context;
        _storeValidator = storeValidator;
    }

    public async Task<ApiResponseDto<Store>> CreateStore(StoreDTO storeDto)
    {
        ValidationResult validationResult = _storeValidator.Validate(storeDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<Store>.ErrorResult(
                "Invalid store data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        Store newStore = new Store { Name = storeDto.Name, UserId = storeDto.UserId };

        await _context.Stores.AddAsync(newStore);

        await _context.SaveChangesAsync();

        return ApiResponseDto<Store>.SuccessResult(newStore);
    }

    public async Task<ApiResponseDto<Store>> GetStoreById(int id)
    {
        Store? store = await _context.Stores.FirstOrDefaultAsync(i => i.Id == id);

        if (store == null)
        {
            return ApiResponseDto<Store>.ErrorResult("Store not found.");
        }

        return ApiResponseDto<Store>.SuccessResult(store);
    }

    public async Task<ApiResponseDto<List<Store>>> GetStoresByUserId(string userId)
    {
        List<Store> stores = await _context.Stores.Where(s => s.UserId == userId).ToListAsync();

        if (stores.Count == 0)
        {
            return ApiResponseDto<List<Store>>.ErrorResult("No stores found for this user.");
        }

        return ApiResponseDto<List<Store>>.SuccessResult(stores);
    }

    public async Task<ApiResponseDto<Store>> UpdateStore(int id, StoreDTO storeDto)
    {
        ValidationResult validationResult = _storeValidator.Validate(storeDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<Store>.ErrorResult(
                "Invalid store data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        Store? existingStore = await _context.Stores.FirstOrDefaultAsync(i => i.Id == id);

        if (existingStore == null)
        {
            return ApiResponseDto<Store>.ErrorResult("Store not found.");
        }

        existingStore.Name = storeDto.Name;
        existingStore.UserId = storeDto.UserId;

        await _context.SaveChangesAsync();

        return ApiResponseDto<Store>.SuccessResult(existingStore);
    }
}
