using System.Security.Claims;
using backend.Infrastructure.Repositories;
using backend.Models;

namespace backend.Services.Auth;

using System.IdentityModel.Tokens.Jwt;

public class CurrentUser : ICurrentUser
{
    public User Value { get; }
    public bool IsAuthenticated { get; }

    public CurrentUser(IHttpContextAccessor accessor, IUserRepository userRepository)
    {
        var principal = accessor.HttpContext?.User;

        if (principal == null || !principal.Identity?.IsAuthenticated == true)
        {
            IsAuthenticated = false;
            return;
        }

        IsAuthenticated = true;

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)
                          ?? throw new UnauthorizedAccessException("Missing sub claim");

        var guid = Guid.Parse(userIdClaim.Value);

        Value = userRepository.GetAsync(guid).Result!;
    }
}
