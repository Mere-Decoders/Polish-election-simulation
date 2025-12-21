using backend.Data;
using backend.Infrastructure.Entities;
using backend.Infrastructure.Helpers;
using backend.Infrastructure.Repositories;
using backend.Models;

namespace backend.Services;

public class SimDataService:ISimDataService
{
    private readonly IDataClaimRepository dataClaimRepository;
    public async Task<SimulationData> GetSimDataByGuid(Guid dataGuid)
    {
        switch (dataGuid.ToString())
        {
            case ("00000000-0000-0000-0000-000000000000"):
                return SimulationDataFromCsv.Get();
            case ("00000000-0000-0000-0000-000000000001"):
                return SimulationDataFromCsv.Get2();
            default:
                throw new KeyNotFoundException("The data you are requesting doesn't exist");
        }
    }

    public async Task<List<DataClaim>> GetUsersSimData(Guid userGuid)
    {
        return await dataClaimRepository.GetUserDataClaims(userGuid);
    }

}