using backend.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories;

public class MethodClaimRepository : IMethodClaimRepository
{
    private readonly AppDbContext _db;

    public MethodClaimRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<MethodClaim>> GetUserMethodClaims(Guid userId)
    {
        return await _db.MethodClaims.Where(r => r.UserId == userId).ToListAsync();
    }

    public async Task<MethodClaim?> CheckExistanceAsync(Guid userId, Guid methodId)
    {
        return await _db.MethodClaims
            .AsNoTracking()
            .Where(c => c.UserId == userId && c.MethodId == methodId)
            .Select(c => new MethodClaim
            {
                UserId = c.UserId,
                MethodId = c.MethodId,
                Label = c.Label
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<MethodClaim>> GetAllAsync()
    {
        return await _db.MethodClaims
            .AsNoTracking()
            .Select(c => new MethodClaim
            {
                UserId = c.UserId,
                MethodId = c.MethodId,
                Label = c.Label
            })
            .ToListAsync();
    }

    public async Task<MethodClaim> AddAsync(Guid userId, Guid methodId, string label)
    {
        var entity = new MethodClaim
        {
            UserId = userId,
            MethodId = methodId,
            Label = label
        };
        _db.MethodClaims.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid userId, Guid methodId)
    {
        var entity = await _db.MethodClaims.FindAsync(userId, methodId);
        if (entity == null)
            return;
        _db.MethodClaims.Remove(entity);
        await _db.SaveChangesAsync();
    }
}