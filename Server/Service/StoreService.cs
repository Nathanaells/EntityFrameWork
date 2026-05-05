using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Implemented_MVC.DTOs;
using Implemented_MVC.Service.Interfaces;
using Server.Repository.Interfaces;

public class StoreService : IStoreService
{
    private readonly AppDbContext _context;
    private readonly IValidator<StoreDTO> _storeValidator;
    private readonly IStoreRepository _storeRepository;

    private readonly IMapper _mapper;

    public StoreService(
        AppDbContext context,
        IValidator<StoreDTO> storeValidator,
        IStoreRepository storeRepository,
        IMapper mapper
    )
    {
        _context = context;
        _storeValidator = storeValidator;
        _storeRepository = storeRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<StoreResponseDTO>> CreateStore(StoreDTO storeDto, string userId)
    {
        ValidationResult validationResult = await _storeValidator.ValidateAsync(storeDto);

        if (!validationResult.IsValid)
        {
            return ServiceResult<StoreResponseDTO>.ErrorResult(
                "Invalid store data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        // Map StoreDTO to Store entity
        Store newStore = _mapper.Map<Store>(storeDto);
        newStore.UserId = userId;

        await _storeRepository.CreateStoreAsync(newStore);

        StoreResponseDTO response = _mapper.Map<StoreResponseDTO>(newStore);

        return ServiceResult<StoreResponseDTO>.SuccessResult(
            response,
            "Store created successfully."
        );
    }

    public async Task<ServiceResult<StoreResponseDTO>> GetStoreById(int id, string userId)
    {
        Store? store = await _storeRepository.GetStoreByIdAsync(id);

        if (store == null)
        {
            return ServiceResult<StoreResponseDTO>.ErrorResult(
                "Store not found.",
                new List<string> { "Store with the provided ID does not exist." }
            );
        }

        if (store.UserId != userId)
        {
            return ServiceResult<StoreResponseDTO>.ErrorResult(
                "Access denied.",
                new List<string> { "You are not the owner of this store." }
            );
        }

        StoreResponseDTO response = _mapper.Map<StoreResponseDTO>(store);

        return ServiceResult<StoreResponseDTO>.SuccessResult(
            response,
            "Successfully retrieved store."
        );
    }

    public async Task<ServiceResult<List<StoreResponseDTO>>> GetStoresByUserId(string userId)
    {
        List<Store> stores = await _storeRepository.GetStoresByUserIdAsync(userId);

        if (stores.Count == 0)
        {
            return ServiceResult<List<StoreResponseDTO>>.ErrorResult(
                "No stores found for this user.",
                new List<string> { "No stores found for the provided user ID." }
            );
        }

        List<StoreResponseDTO> response = _mapper.Map<List<StoreResponseDTO>>(stores).ToList();

        return ServiceResult<List<StoreResponseDTO>>.SuccessResult(response);
    }

    public async Task<ServiceResult<StoreResponseDTO>> UpdateStore(
        UpdateStoreDTO req,
        string userId
    )
    {
        Store? store = await _storeRepository.GetStoreByIdAsync(req.Id);

        if (store == null)
        {
            return ServiceResult<StoreResponseDTO>.ErrorResult(
                "Store not found.",
                new List<string> { "Store with the provided ID does not exist." }
            );
        }

        if (store.UserId != userId)
        {
            return ServiceResult<StoreResponseDTO>.ErrorResult(
                "Access denied.",
                new List<string> { "You are not the owner of this store." }
            );
        }

        store.Name = req.Name;
        store.Location = req.Location;

        await _storeRepository.UpdateStoreAsync(store);

        StoreResponseDTO response = _mapper.Map<StoreResponseDTO>(store);

        return ServiceResult<StoreResponseDTO>.SuccessResult(
            response,
            "Store updated successfully."
        );
    }

    public async Task<ServiceResult<bool>> DeleteStore(int id, string userId)
    {
        Store? store = await _storeRepository.GetStoreByIdAsync(id);

        if (store == null)
        {
            return ServiceResult<bool>.ErrorResult(
                "Store not found.",
                new List<string> { "Store with the provided ID does not exist." }
            );
        }

        if (store.UserId != userId)
        {
            return ServiceResult<bool>.ErrorResult(
                "Access denied.",
                new List<string> { "You are not the owner of this store." }
            );
        }

        await _storeRepository.DeleteStoreAsync(store.Id);

        return ServiceResult<bool>.SuccessResult(true);
    }
}
