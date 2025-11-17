using backend.Data;

namespace backend.Services;

public interface IMethodManager
{
    public Func<SimulationData, ElectionResult> GetMethodByGuid(Guid methodGuid);


}