namespace Server.Service;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Implemented_MVC.DTOs;
using Implemented_MVC.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;

    private readonly JWtOptions _jwtOptions;
    private readonly IValidator<RegisterDTO> _registerValidator;
    private readonly IValidator<LoginDTO> _loginValidator;
    private readonly IValidator<UpdateUserDTO> _updateUserValidator;

    private readonly IMapper _mapper;

    public AuthService(
        UserManager<User> userManager,
        IValidator<RegisterDTO> registerValidator,
        IValidator<LoginDTO> loginValidator,
        IValidator<UpdateUserDTO> updateUserValidator,
        IOptions<JWtOptions> jwtOptions,
        IMapper mapper
    )
    {
        _userManager = userManager;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _updateUserValidator = updateUserValidator;
        _jwtOptions = jwtOptions.Value;
        _mapper = mapper;
    }

    public async Task<ServiceResult<RegisterResponseDTO>> RegisterAsync(RegisterDTO registerDto)
    {
        ValidationResult validationResult = _registerValidator.Validate(registerDto);

        if (!validationResult.IsValid)
        {
            return ServiceResult<RegisterResponseDTO>.ErrorResult(
                "Invalid registration data.",
                validationResult.Errors.Select(e => e.ErrorMessage).ToList()
            );
        }

        User? existingUser = await _userManager.FindByEmailAsync(registerDto.Email);

        if (existingUser != null)
        {
            return ServiceResult<RegisterResponseDTO>.ErrorResult(
                "Email already in use.",
                new List<string> { "A user with this email already exists." }
            );
        }

        User user = _mapper.Map<User>(registerDto);

        IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return ServiceResult<RegisterResponseDTO>.ErrorResult(
                "User registration failed.",
                result.Errors.Select(e => e.Description).ToList()
            );
        }

        RegisterResponseDTO response = _mapper.Map<RegisterResponseDTO>(user);

        return ServiceResult<RegisterResponseDTO>.SuccessResult(
            response,
            "User registered successfully."
        );
    }

    public async Task<ServiceResult<LoginResponseDTO>> LoginAsync(LoginDTO loginDto)
    {
        try
        {
            ValidationResult validationResult = _loginValidator.Validate(loginDto);

            if (!validationResult.IsValid)
            {
                return ServiceResult<LoginResponseDTO>.ErrorResult(
                    "Invalid login data.",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                );
            }

            User? user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return ServiceResult<LoginResponseDTO>.ErrorResult(
                    "Login failed.",
                    new List<string> { "Invalid email or password." }
                );
            }

            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
            {
                return ServiceResult<LoginResponseDTO>.ErrorResult(
                    "Login failed.",
                    new List<string> { "Invalid email or password." }
                );
            }

            string token = GenerateJwtToken(user);

            LoginResponseDTO response = new LoginResponseDTO
            {
                Token = token,
                UserId = user.Id,
                Username = user.DisplayName,
            };

            return ServiceResult<LoginResponseDTO>.SuccessResult(response, "Login successful.");
        }
        catch (Exception ex)
        {
            return ServiceResult<LoginResponseDTO>.ErrorResult(
                ex.Message,
                new List<string> { "Unexpected error occurred during login." }
            );
        }
    }

    private string GenerateJwtToken(User user)
    {
        string jwtKey = _jwtOptions.Key;
        string jwtIssuer = _jwtOptions.Issuer;
        string jwtAudience = _jwtOptions.Audience;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
