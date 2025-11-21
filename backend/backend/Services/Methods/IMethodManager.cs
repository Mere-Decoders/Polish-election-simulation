using backend.Data;

namespace backend.Services.Methods;

public interface IMethodManager
{
    public Func<SimulationData, ElectionResult> GetMethodByGuid(Guid methodGuid);


}