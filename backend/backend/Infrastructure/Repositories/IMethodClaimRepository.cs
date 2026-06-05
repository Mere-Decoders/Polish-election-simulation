using backend.Infrastructure.Entities;

namespace backend.Infrastructure.Repositories;

public interface IMethodClaimRepository
{
    Task<List<MethodClaim>> GetUserMethodClaims(Guid userId);
    Task<MethodClaim?> CheckExistanceAsync(Guid userId, Guid methodId);
    Task<IEnumerable<MethodClaim>> GetAllAsync();
    Task<MethodClaim> AddAsync(Guid userId, Guid methodId, string label);
    Task DeleteAsync(Guid userId, Guid methodId);
}