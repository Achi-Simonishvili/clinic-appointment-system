using ClinicSystem.Application.DTOs.Auth;
using ClinicSystem.Application.Interfaces;
using ClinicSystem.Domain.Entities;
using ClinicSystem.Domain.Enums;

namespace ClinicSystem.Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IJwtService jwtService, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
            throw new Exception("Email already in use.");

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = UserRole.Patient.ToString()
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return new AuthResponse
        {
            Token = _jwtService.GenerateToken(user),
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = user.Role
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new Exception("Invalid email or password.");

        return new AuthResponse
        {
            Token = _jwtService.GenerateToken(user),
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = user.Role
        };
    }
}
