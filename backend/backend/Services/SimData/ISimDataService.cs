using backend.Data;
using backend.Infrastructure.Entities;

namespace backend.Services;

public interface ISimDataService
{
    public Task<SimulationData> GetSimDataByGuid(Guid simdataGuid);

    public Task<List<DataClaim>> GetUsersSimData(Guid userGuid);
}