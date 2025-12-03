using backend.Data;
using backend.Services.Methods;
using Microsoft.AspNetCore.Mvc;

namespace backend.Services;

public class SimContextService : ISimContextService
{
    private readonly ISimDataService _simDataService;
    private readonly IMethodService _methodService;

    public SimContextService(ISimDataService simDataService, IMethodService methodService)
    {
        _simDataService = simDataService;
        _methodService = methodService;
    }
    public async Task<SimContext> FetchContextByGuid(Guid simDataGuid, Guid methodGuid)
    {
        var simData = await _simDataService.GetSimDataByGuid(methodGuid);
        return new SimContext(_methodService.GetMethodByGuid(methodGuid), simData);
    }
    
}