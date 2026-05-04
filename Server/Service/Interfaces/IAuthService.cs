namespace Implemented_MVC.Service.Interfaces;

using Implemented_MVC.DTOs;

public interface IAuthService
{
    public Task<ApiResponseDto<RegisterResponseDTO>> RegisterAsync(RegisterDTO registerDto);
    public Task<ApiResponseDto<LoginResponseDTO>> LoginAsync(LoginDTO loginDto);
}
