using backend.Data;
using backend.Services.Methods;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services;

public class SimContextService : ISimContextService
{
    private readonly ISimDataService _simDataService;
    private readonly IMethodManager _methodManager;

    public SimContextService(ISimDataService simDataService, IMethodManager methodManager)
    {
        _simDataService = simDataService;
        _methodManager = methodManager;
    }
    public async Task<SimContext> FetchContextByGuid(Guid simDataGuid, Guid methodGuid)
    {
        var simData = await _simDataService.GetSimDataByGuid(methodGuid);
        return new SimContext(_methodManager.GetMethodByGuid(methodGuid), simData);
    }
    
}