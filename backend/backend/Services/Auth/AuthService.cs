using backend.Infrastructure.Repositories;
using backend.Models;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task RegisterAsync(string email, string username, string password)
    {
        if (await _userRepository.ExistsByUsernameAsync(username))
            throw new InvalidOperationException("Username already taken");

        if (await _userRepository.ExistsByEmailAsync(email))
            throw new InvalidOperationException("Email already taken");

        //password hashing
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        await _userRepository.AddAsync(
            Guid.NewGuid(),
            email,
            username,
            passwordHash
        );
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
