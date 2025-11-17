using backend.Data;

namespace backend.Services;

public interface ISimDataManager
{
    public SimulationData GetSimDataByGuid(Guid simdataGuid);
}