using backend.Data;

namespace backend.Services;

public class SimulationService:ISimulationService
{
    public readonly ISimContextService SimContextService;

    public SimulationService(ISimContextService simContextService)
    {
        SimContextService = simContextService;
    }

    public async Task<ElectionResult> Simulate(Guid simDataGuid, Guid methodGuid)
    {
        var context =  await SimContextService.FetchContextByGuid(simDataGuid, methodGuid);
        var res = context.CountingMethod(context.Data);
        return res;
    }
}