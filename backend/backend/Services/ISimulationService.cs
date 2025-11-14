using backend.Data;

namespace backend.Services;

public interface ISimulationService
{
    public ElectionResult Simulate(Guid SimDataGuid, Guid MethodGuid );
}