using backend.Infrastructure.Entities;

namespace backend.Infrastructure.Repositories;

public interface IDataClaimRepository
{
    Task<DataClaim?> CheckExistanceAsync(Guid userId, Guid dataId);
    Task<IEnumerable<DataClaim>> GetAllAsync();
    Task<DataClaim> AddAsync(Guid userId, Guid dataId);
    Task DeleteAsync(Guid userId, Guid dataId);
}