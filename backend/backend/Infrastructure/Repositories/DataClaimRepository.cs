using backend.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories;

public class DataClaimRepository : IDataClaimRepository
{

    private readonly AppDbContext _db;

    public DataClaimRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DataClaim?> CheckExistanceAsync(Guid userId, Guid dataId)
    {
        return await _db.DataClaims
        .AsNoTracking()
        .Where(c => c.UserId == userId && c.DataId == dataId)
        .Select(c => new DataClaim
        {
            UserId = c.UserId,
            DataId = c.DataId
        })
        .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<DataClaim>> GetAllAsync()
    {
        return await _db.DataClaims
        .AsNoTracking()
        .Select(c => new DataClaim
        {
            UserId = c.UserId,
            DataId = c.DataId
        })
        .ToListAsync();
    }

    public async Task<DataClaim> AddAsync(Guid userId, Guid dataId)
    {
        var entity = new DataClaim
        {
            UserId = userId,
            DataId = dataId
        };

        _db.DataClaims.Add(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public async Task DeleteAsync(Guid userId, Guid dataId)
    {
        var entity = await _db.DataClaims.FindAsync(userId, dataId);
        if (entity == null)
            return;

        _db.DataClaims.Remove(entity);
        await _db.SaveChangesAsync();
    }

}