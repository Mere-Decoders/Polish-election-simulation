using backend.Infrastructure.Entities;
using backend.Models;

namespace backend.Services;

public interface ISimMethodService
{
    Task<Method> GetMethodByGuid(Guid userId, Guid methodGuid);
    Task<Method> GetMethodByGuidBackendOnly(Guid methodGuid);
    Task<List<MethodClaim>> GetUserMethods(Guid userGuid);
    Task<MethodClaim> CreateMethodForUserAsync(Guid userId, string label, Method method);
    Task UpdateMethodForUserAsync(Guid userId, Guid methodId, Method method);
    Task DeleteMethodForUserAsync(Guid userId, Guid methodId);
}