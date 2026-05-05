using Implemented_MVC.DTOs;

namespace Implemented_MVC.Service.Interfaces;

public interface IUserService
{
    public Task<ServiceResult<UserResponseDTO>> GetCurrentUserAsync(string userId);
    public Task<ServiceResult<UserResponseDTO>> UpdateCurrentUserAsync(
        string userId,
        UpdateUserDTO updateUserDto
    );
}
