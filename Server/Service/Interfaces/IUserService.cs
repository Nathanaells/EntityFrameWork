using Implemented_MVC.DTOs;

namespace Implemented_MVC.Service.Interfaces;

public interface IUserService
{
    public Task<ApiResponseDto<UserResponseDTO>> GetCurrentUserAsync(string userId);
    public Task<ApiResponseDto<UserResponseDTO>> UpdateCurrentUserAsync(
        string userId,
        UpdateUserDTO updateUserDto
    );
}
