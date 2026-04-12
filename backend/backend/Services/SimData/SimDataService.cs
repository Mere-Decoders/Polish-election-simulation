using backend.Data;
using backend.Infrastructure.Entities;
using backend.Infrastructure.Repositories;
using backend.Models;

namespace backend.Services;

public class SimDataService:ISimDataService
{
    private static readonly Guid WiarygodneWyboryTemplateId =
        Guid.Parse("00000000-0000-0000-0000-000000000001");

    private readonly IDataClaimRepository _dataClaimRepository;
    private readonly ISimDataRepository _simDataRepository;

    public SimDataService(IDataClaimRepository dataClaimRepository, ISimDataRepository simDataRepository)
    {
        _dataClaimRepository = dataClaimRepository;
        _simDataRepository = simDataRepository;
    }
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

    public async Task<DataClaim> CreateSimDataForUserAsync(Guid userId, string label, SimulationData data)
    {
        var id = Guid.NewGuid();
        await _simDataRepository.AddAsync(id, data);
        return await _dataClaimRepository.AddAsync(userId, id, label);
    }

    public async Task UpdateSimDataForUserAsync(Guid userId, Guid dataId, SimulationData data)
    {
        if (dataId == Guid.Empty || dataId == WiarygodneWyboryTemplateId)
            throw new UnauthorizedAccessException("This simulation dataset cannot be modified.");

        var claim = await _dataClaimRepository.CheckExistanceAsync(userId, dataId);
        if (claim == null)
            throw new UnauthorizedAccessException("You do not have access to this simulation data.");

        await _simDataRepository.UpdateAsync(dataId, data);
    }

}