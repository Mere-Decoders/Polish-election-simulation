using backend.Data;
using backend.Infrastructure.Entities;
using backend.Infrastructure.Repositories;
using backend.Services;

public class SimDataService : ISimDataService
{
    private static readonly IReadOnlySet<Guid> ProtectedDataIds = new HashSet<Guid>
    {
        Guid.Parse("00000000-0000-0000-0000-000000000000"),
        Guid.Parse("00000000-0000-0000-0000-000000000001"),
        Guid.Parse("00000000-0000-0000-0000-000000000002"),
        Guid.Parse("00000000-0000-0000-0000-000000000003"),
        Guid.Parse("00000000-0000-0000-0000-000000000004"),
    };

    private readonly IDataClaimRepository _dataClaimRepository;
    private readonly ISimDataRepository _simDataRepository;

    public SimDataService(IDataClaimRepository dataClaimRepository, ISimDataRepository simDataRepository)
    {
        _dataClaimRepository = dataClaimRepository;
        _simDataRepository = simDataRepository;
    }

    public async Task<SimulationData> GetSimDataByGuid(Guid userId, Guid dataGuid)
    {
        var claim = await _dataClaimRepository.CheckExistanceAsync(userId, dataGuid);
        if (claim == null)
            throw new UnauthorizedAccessException("You do not have access to this data.");

        var result = await _simDataRepository.GetAsync(dataGuid);
        if (result != null) return result;
        throw new KeyNotFoundException("The data you are requesting doesn't exist");
    }

    public async Task<SimulationData> GetSimDataByGuidBackendOnly(Guid dataGuid)
    {
        var result = await _simDataRepository.GetAsync(dataGuid);
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
        if (ProtectedDataIds.Contains(dataId))
            throw new UnauthorizedAccessException("This simulation dataset cannot be modified.");

        var claim = await _dataClaimRepository.CheckExistanceAsync(userId, dataId);
        if (claim == null)
            throw new UnauthorizedAccessException("You do not have access to this simulation data.");

        await _simDataRepository.UpdateAsync(dataId, data);
    }

    public async Task DeleteSimDataForUserAsync(Guid userId, Guid dataId)
    {
        if (ProtectedDataIds.Contains(dataId))
            throw new UnauthorizedAccessException("This simulation dataset cannot be deleted.");

        var claim = await _dataClaimRepository.CheckExistanceAsync(userId, dataId);
        if (claim == null)
            throw new UnauthorizedAccessException("You do not have access to this simulation data.");

        await _simDataRepository.DeleteAsync(dataId);
        await _dataClaimRepository.DeleteAsync(userId, dataId);
    }
}