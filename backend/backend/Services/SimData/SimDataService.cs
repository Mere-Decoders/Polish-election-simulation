using backend.Data;
using backend.Infrastructure.Helpers;
using backend.Models;

namespace backend.Services;

public class SimDataService:ISimDataService
{
    public async Task<SimulationData> GetSimDataByGuid(Guid dataGuid)
    {
        switch (dataGuid.ToString())
        {
            case ("00000000-0000-0000-0000-000000000000"):
                return SimulationDataFromCsv.Get();
            case ("00000000-0000-0000-0000-000000000001"):
                return SimulationDataFromCsv.Get2();
            default:
                throw new ArgumentException("The method you are requesting doesn't exist");
        }
    }

    public async Task<List<SimulationData>> GetUsersSimData(Guid userGuid)
    {
        throw new NotImplementedException();
    }

}