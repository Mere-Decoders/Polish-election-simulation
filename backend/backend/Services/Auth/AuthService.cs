using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Services.Auth;

using backend.Infrastructure.Repositories;
using backend.Models;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IDataClaimRepository _claimRepository;
    private readonly IMethodClaimRepository _methodRepository;
    public AuthService(IUserRepository userRepository, IDataClaimRepository dataClaimRepository, IMethodClaimRepository methodClaimRepository)
    {
        _userRepository = userRepository;
        _claimRepository = dataClaimRepository;
        _methodRepository = methodClaimRepository;
    }

    public async Task RegisterAsync(string email, string username, string password)
    {
        if (await _userRepository.ExistsByUsernameAsync(username))
            throw new InvalidOperationException("Username already taken");

        if (await _userRepository.ExistsByEmailAsync(email))
            throw new InvalidOperationException("Email already taken");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        
        Guid guid = Guid.NewGuid();
        await _userRepository.AddAsync(
            guid,
            email,
            username,
            passwordHash
        );
        
        await _claimRepository.AddAsync(guid, Guid.Parse("00000000-0000-0000-0000-000000000003"), "Wybory 2019");
        await _claimRepository.AddAsync(guid, Guid.Parse("00000000-0000-0000-0000-000000000004"), "Wybory 2023");
        await _methodRepository.AddAsync(guid, Guid.Empty, "Dhond't method");
        await _methodRepository.AddAsync(guid, Guid.Parse("00000000-0000-0000-0000-000000000001"), "High-stakes method");
        await _methodRepository.AddAsync(guid, Guid.Parse("00000000-0000-0000-0000-000000000002"), "Sainte-Lague method");

    }


    public async Task<User?> LoginAsync(string username, string password)
    {
        var userAuth = await _userRepository.GetForAuthByUsernameAsync(username);

        if (userAuth == null)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(password, userAuth.PasswordHash))
            return null;

        return await _userRepository.GetAsync(userAuth.Id);
    }

}
