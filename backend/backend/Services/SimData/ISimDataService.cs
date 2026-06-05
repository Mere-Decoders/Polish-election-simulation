using backend.Data;
using backend.Infrastructure.Entities;

namespace backend.Services;

public interface ISimDataService
{
    Task<SimulationData> GetSimDataByGuid(Guid userId, Guid dataGuid);

    Task<SimulationData> GetSimDataByGuidBackendOnly(Guid dataGuid);

    public Task<List<DataClaim>> GetUsersSimData(Guid userGuid);

    public Task<DataClaim> CreateSimDataForUserAsync(Guid userId, string label, SimulationData data);

    public Task UpdateSimDataForUserAsync(Guid userId, Guid dataId, SimulationData data);
    Task DeleteSimDataForUserAsync(Guid userId, Guid dataId);
}