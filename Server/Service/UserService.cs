using FluentValidation;
using FluentValidation.Results;
using Implemented_MVC.DTOs;
using Microsoft.AspNetCore.Identity;

public class UserService
{
    private readonly UserManager<User> _userManager;
    private readonly IValidator<UpdateUserDTO> _updateUserValidator;

    public UserService(UserManager<User> userManager, IValidator<UpdateUserDTO> updateUserValidator)
    {
        _userManager = userManager;
        _updateUserValidator = updateUserValidator;
    }

    public async Task<ApiResponseDto<User>> GetCurrentUserAsync(string userId)
    {
        User? user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return ApiResponseDto<User>.ErrorResult("User not found.", new List<string> { "User with the provided ID does not exist." });
        }

        return ApiResponseDto<User>.SuccessResult(user);
    }

    public async Task<ApiResponseDto<User>> UpdateCurrentUserAsync(
        string userId,
        UpdateUserDTO updateUserDto
    )
    {
        if (string.IsNullOrWhiteSpace(updateUserDto.Username)
            && string.IsNullOrWhiteSpace(updateUserDto.Password))
        {
            return ApiResponseDto<User>.ErrorResult(
                "No data to update.",
                new List<string> { "Provide at least one field to update." }
            );
        }

        ValidationResult validationResult = _updateUserValidator.Validate(updateUserDto);

        if (!validationResult.IsValid)
        {
            return ApiResponseDto<User>.ErrorResult(
                "Invalid user data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        User? user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return ApiResponseDto<User>.ErrorResult(
                "User update failed.",
                new List<string> { "User not found." }
            );
        }


        user.DisplayName = updateUserDto.Username;
        user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, updateUserDto.Password);


        return ApiResponseDto<User>.SuccessResult(user, "User updated successfully.");
    }
}
