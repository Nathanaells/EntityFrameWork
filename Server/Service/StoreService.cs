using FluentValidation;
using FluentValidation.Results;
using Implemented_MVC.DTOs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

public class StoreService
{
    private readonly AppDbContext _context;
    private readonly IValidator<StoreDTO> _storeValidator;

    private readonly IMapper _mapper;

    public StoreService(AppDbContext context, IValidator<StoreDTO> storeValidator, IMapper mapper)
    {
        _context = context;
        _storeValidator = storeValidator;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto<StoreResponseDTO>> CreateStore(StoreDTO storeDto, string userId)
    {
        ValidationResult validationResult = await _storeValidator.ValidateAsync(storeDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<StoreResponseDTO>.ErrorResult(
                "Invalid store data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }


        // Map StoreDTO to Store entity
        Store newStore = _mapper.Map<Store>(storeDto);
        newStore.UserId = userId;

        await _context.Stores.AddAsync(newStore);

        StoreResponseDTO response = _mapper.Map<StoreResponseDTO>(newStore);

        await _context.SaveChangesAsync();

        return ApiResponseDto<StoreResponseDTO>.SuccessResult(response, "Store created successfully.");
    }

    public async Task<ApiResponseDto<StoreResponseDTO>> GetStoreById(int id, string userId)
    {
        Store? store = await _context.Stores.FirstOrDefaultAsync(i => i.Id == id);

        if (store == null)
        {
            return ApiResponseDto<StoreResponseDTO>.ErrorResult("Store not found.", new List<string> { "Store with the provided ID does not exist." });
        }

        if (store.UserId != userId)
        {
            return ApiResponseDto<StoreResponseDTO>.ErrorResult("Access denied.", new List<string> { "You are not the owner of this store." });
        }

        StoreResponseDTO response = _mapper.Map<StoreResponseDTO>(store);

        return ApiResponseDto<StoreResponseDTO>.SuccessResult(response, $"Store Owned by user : {userId}.");
    }

    public async Task<ApiResponseDto<List<StoreResponseDTO>>> GetStoresByUserId(string userId)
    {
        List<Store> stores = await _context.Stores.Where(s => s.UserId == userId).ToListAsync();

        if (stores.Count == 0)
        {
            return ApiResponseDto<List<StoreResponseDTO>>.ErrorResult(
                "No stores found for this user.",
                new List<string> { "No stores found for the provided user ID." }
            );
        }


        List<StoreResponseDTO> response = _mapper.Map<List<StoreResponseDTO>>(stores).ToList();

        return ApiResponseDto<List<StoreResponseDTO>>.SuccessResult(response);
    }

    public async Task<ApiResponseDto<bool>> DeleteStore(int id, string userId)
    {
        Store? store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == id);

        if (store == null)
        {
            return ApiResponseDto<bool>.ErrorResult("Store not found.", new List<string> { "Store with the provided ID does not exist." });
        }

        if (store.UserId != userId)
        {
            return ApiResponseDto<bool>.ErrorResult("Access denied.", new List<string> { "You are not the owner of this store." });
        }

        _context.Stores.Remove(store);
        await _context.SaveChangesAsync();

        return ApiResponseDto<bool>.SuccessResult(true);
    }


}
