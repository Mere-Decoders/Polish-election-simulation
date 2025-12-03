using backend.Data;

namespace backend.Services.Methods;

public interface IMethodService
{
    public Func<SimulationData, ElectionResult> GetMethodByGuid(Guid methodGuid);


}