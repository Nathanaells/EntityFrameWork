namespace Implemented_MVC.Service.Interfaces;

using Implemented_MVC.DTOs;

public interface IAuthService
{
    public Task<ServiceResult<RegisterResponseDTO>> RegisterAsync(RegisterDTO registerDto);
    public Task<ServiceResult<LoginResponseDTO>> LoginAsync(LoginDTO loginDto);
}
