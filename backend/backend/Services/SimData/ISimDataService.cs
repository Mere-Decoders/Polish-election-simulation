using backend.Data;

namespace backend.Services;

public interface ISimDataService
{
    public Task<SimulationData> GetSimDataByGuid(Guid simdataGuid);

    public Task<List<SimulationData>> GetUsersSimData(Guid userGuid);
}