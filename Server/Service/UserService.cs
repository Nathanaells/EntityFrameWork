using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Implemented_MVC.DTOs;
using Implemented_MVC.Service.Interfaces;
using Microsoft.AspNetCore.Identity;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IValidator<UpdateUserDTO> _updateUserValidator;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<User> userManager,
        IValidator<UpdateUserDTO> updateUserValidator,
        IMapper mapper
    )
    {
        _userManager = userManager;
        _updateUserValidator = updateUserValidator;
        _mapper = mapper;
    }

    public async Task<ApiResponseDto<UserResponseDTO>> GetCurrentUserAsync(string userId)
    {
        User? user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return ApiResponseDto<UserResponseDTO>.ErrorResult(
                "User not found.",
                new List<string> { "User with the provided ID does not exist." }
            );
        }

        UserResponseDTO userResponse = _mapper.Map<UserResponseDTO>(user);

        return ApiResponseDto<UserResponseDTO>.SuccessResult(userResponse);
    }

    public async Task<ApiResponseDto<UserResponseDTO>> UpdateCurrentUserAsync(
        string userId,
        UpdateUserDTO updateUserDto
    )
    {
        if (
            string.IsNullOrWhiteSpace(updateUserDto.Username)
            && string.IsNullOrWhiteSpace(updateUserDto.Password)
        )
        {
            return ApiResponseDto<UserResponseDTO>.ErrorResult(
                "No data to update.",
                new List<string> { "Provide at least one field to update." }
            );
        }

        ValidationResult validationResult = _updateUserValidator.Validate(updateUserDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<UserResponseDTO>.ErrorResult(
                "Invalid user data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        User? user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return ApiResponseDto<UserResponseDTO>.ErrorResult(
                "User update failed.",
                new List<string> { "User not found." }
            );
        }

        if (!string.IsNullOrWhiteSpace(updateUserDto.Username))
        {
            user.DisplayName = updateUserDto.Username;
        }

        if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
        {
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, updateUserDto.Password);
        }

        IdentityResult updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            return ApiResponseDto<UserResponseDTO>.ErrorResult(
                "User update failed.",
                updateResult.Errors.Select(e => e.Description).ToList()
            );
        }

        UserResponseDTO userResponse = _mapper.Map<UserResponseDTO>(user);

        return ApiResponseDto<UserResponseDTO>.SuccessResult(
            userResponse,
            "User updated successfully."
        );
    }
}
