using backend.Data;
using backend.Infrastructure.Helpers;

namespace backend.Services;

public class SimDataManager:ISimDataManager
{
    public SimulationData GetSimDataByGuid(Guid dataGuid)
    {
        return SimulationDataFromCsv.Get();
    }
}