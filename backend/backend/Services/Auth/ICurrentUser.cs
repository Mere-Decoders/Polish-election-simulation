using backend.Models;

namespace backend.Services.Auth;

public interface ICurrentUser
{
    User Value { get; }
    bool IsAuthenticated { get; }
}
