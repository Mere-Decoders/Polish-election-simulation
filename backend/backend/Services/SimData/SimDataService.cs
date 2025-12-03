using backend.Data;
using backend.Infrastructure.Helpers;

namespace backend.Services;

public class SimDataService:ISimDataService
{
    public async Task<SimulationData> GetSimDataByGuid(Guid dataGuid)
    {
        return SimulationDataFromCsv.Get();
    }

    public async Task<List<SimulationData>> GetUsersSimData(Guid userGuid)
    {
        throw new NotImplementedException();
    }

}