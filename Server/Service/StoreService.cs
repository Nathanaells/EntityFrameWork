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

    public async Task<ApiResponseDto<StoreResponseDTO>> CreateStore(StoreDTO storeDto, string userId)
    {
        ValidationResult validationResult = _storeValidator.Validate(storeDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<StoreResponseDTO>.ErrorResult(
                "Invalid store data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }


        Store newStore = new Store
        {
            Name = storeDto.Name,
            Location = storeDto.Location,
            UserId = userId,

        };

        await _context.Stores.AddAsync(newStore);

        await _context.SaveChangesAsync();

        return ApiResponseDto<StoreResponseDTO>.SuccessResult(MapStoreResponse(newStore));
    }

    public async Task<ApiResponseDto<StoreResponseDTO>> GetStoreById(int id, string userId)
    {
        Store? store = await _context.Stores.FirstOrDefaultAsync(i => i.Id == id);

        if (store == null)
        {
            return ApiResponseDto<StoreResponseDTO>.ErrorResult("Store not found.");
        }

        if (store.UserId != userId)
        {
            return ApiResponseDto<StoreResponseDTO>.ErrorResult("Access denied.");
        }

        return ApiResponseDto<StoreResponseDTO>.SuccessResult(MapStoreResponse(store));
    }

    public async Task<ApiResponseDto<List<StoreResponseDTO>>> GetStoresByUserId(string userId)
    {
        List<Store> stores = await _context.Stores.Where(s => s.UserId == userId).ToListAsync();

        if (stores.Count == 0)
        {
            return ApiResponseDto<List<StoreResponseDTO>>.ErrorResult(
                "No stores found for this user."
            );
        }

        List<StoreResponseDTO> response = stores.Select(MapStoreResponse).ToList();

        return ApiResponseDto<List<StoreResponseDTO>>.SuccessResult(response);
    }

    public async Task<ApiResponseDto<bool>> DeleteStore(int id, string userId)
    {
        Store? store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == id);

        if (store == null)
        {
            return ApiResponseDto<bool>.ErrorResult("Store not found.");
        }

        if (store.UserId != userId)
        {
            return ApiResponseDto<bool>.ErrorResult("Access denied.");
        }

        _context.Stores.Remove(store);
        await _context.SaveChangesAsync();

        return ApiResponseDto<bool>.SuccessResult(true);
    }

    private static StoreResponseDTO MapStoreResponse(Store store)
    {
        return new StoreResponseDTO
        {
            Id = store.Id,
            Name = store.Name,
            Location = store.Location,
        };
    }
}
