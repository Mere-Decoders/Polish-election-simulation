using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Services.Auth;

using backend.Infrastructure.Repositories;
using backend.Models;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IDataClaimRepository _claimRepository;
    public AuthService(IUserRepository userRepository, IDataClaimRepository dataClaimRepository)
    {
        _userRepository = userRepository;
        _claimRepository = dataClaimRepository;
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
        
        await _claimRepository.AddAsync(guid, Guid.Empty, "Wybory 2023");
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
