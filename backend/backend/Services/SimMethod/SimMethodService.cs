using backend.Infrastructure.Entities;
using backend.Infrastructure.Repositories;
using backend.Models;

namespace backend.Services;

public class SimMethodService : ISimMethodService
{
    private readonly IMethodClaimRepository _methodClaimRepository;
    private readonly ISimMethodRepository _simMethodRepository;

    private static readonly IReadOnlySet<Guid> ProtectedMethodIds = new HashSet<Guid>
    {
        Guid.Parse("00000000-0000-0000-0000-000000000000"),
        Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Guid.Parse("00000000-0000-0000-0000-000000000002"),
    };

    public SimMethodService(IMethodClaimRepository methodClaimRepository, ISimMethodRepository simMethodRepository)
    {
        _methodClaimRepository = methodClaimRepository;
        _simMethodRepository = simMethodRepository;
    }

    public async Task<Method> GetMethodByGuid(Guid userId, Guid methodGuid)
    {
        var claim = await _methodClaimRepository.CheckExistanceAsync(userId, methodGuid);
        if (claim == null)
            throw new UnauthorizedAccessException("You do not have access to this method.");

        var result = await _simMethodRepository.GetAsync(methodGuid);
        if (result != null) return result;
        throw new KeyNotFoundException("The method you are requesting doesn't exist.");
    }

    public async Task<Method> GetMethodByGuidBackendOnly(Guid methodGuid)
    {
        var result = await _simMethodRepository.GetAsync(methodGuid);
        if (result != null) return result;
        throw new KeyNotFoundException("The method you are requesting doesn't exist.");
    }

    public async Task<List<MethodClaim>> GetUserMethods(Guid userGuid)
    {
        return await _methodClaimRepository.GetUserMethodClaims(userGuid);
    }

    public async Task<MethodClaim> CreateMethodForUserAsync(Guid userId, string label, Method method)
    {
        var id = Guid.NewGuid();
        await _simMethodRepository.AddAsync(id, method.Name!, method.LuaCode);
        return await _methodClaimRepository.AddAsync(userId, id, label);
    }

    public async Task UpdateMethodForUserAsync(Guid userId, Guid methodId, Method method)
    {
        if (ProtectedMethodIds.Contains(methodId))
            throw new UnauthorizedAccessException("This method cannot be modified.");

        var claim = await _methodClaimRepository.CheckExistanceAsync(userId, methodId);
        if (claim == null)
            throw new UnauthorizedAccessException("You do not have access to this method.");

        await _simMethodRepository.UpdateAsync(methodId, method.Name!, method.LuaCode);
    }

    public async Task DeleteMethodForUserAsync(Guid userId, Guid methodId)
    {
        if (ProtectedMethodIds.Contains(methodId))
            throw new UnauthorizedAccessException("This method cannot be modified.");

        var claim = await _methodClaimRepository.CheckExistanceAsync(userId, methodId);
        if (claim == null)
            throw new UnauthorizedAccessException("You do not have access to this method.");

        await _simMethodRepository.DeleteAsync(methodId);
        await _methodClaimRepository.DeleteAsync(userId, methodId);
    }
}