using backend.Data;

namespace backend.Services;

public class SimulationService:ISimulationService
{
    public readonly ISimContextManager _simContextManager;

    public SimulationService(ISimContextManager simContextManager)
    {
        _simContextManager = simContextManager;
    }

    public ElectionResult Simulate(Guid simDataGuid, Guid methodGuid)
    {
        var context =  _simContextManager.FetchContextByGuid(simDataGuid, methodGuid);
        return context.CountingMethod(context.Data);
    }
}