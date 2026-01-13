using backend.Data;
using backend.Infrastructure.Entities;
using backend.Infrastructure.Helpers;
using backend.Infrastructure.Repositories;
using backend.Models;

namespace backend.Services;

public class SimDataService:ISimDataService
{
    private readonly IDataClaimRepository _dataClaimRepository;
    private readonly ISimDataRepository _simDataRepository;
    public async Task<SimulationData> GetSimDataByGuid(Guid dataGuid)
    {
        var result= await _simDataRepository.GetAsync(dataGuid);
        if (result != null) return result;
        throw new KeyNotFoundException("The data you are requesting doesn't exist");
    }

    public async Task<List<DataClaim>> GetUsersSimData(Guid userGuid)
    {
        return await _dataClaimRepository.GetUserDataClaims(userGuid);
    }

}