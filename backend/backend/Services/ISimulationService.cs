using backend.Data;

namespace backend.Services;

public interface ISimulationService
{
    public Task<ElectionResult> Simulate(Guid SimDataGuid, Guid MethodGuid );
}