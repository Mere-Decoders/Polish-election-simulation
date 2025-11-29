using backend.Data;
using backend.Services.Methods;

namespace backend.Services;

public class SimContextManager : ISimContextManager
{
    private readonly ISimDataManager _simDataManager;
    private readonly IMethodManager _methodManager;

    public SimContextManager(ISimDataManager simDataManager, IMethodManager methodManager)
    {
        _simDataManager = simDataManager;
        _methodManager = methodManager;
    }
    public SimContext FetchContextByGuid(Guid simDataGuid, Guid methodGuid)
    {
        return new SimContext(_methodManager.GetMethodByGuid(methodGuid) , _simDataManager.GetSimDataByGuid(methodGuid));
    }
    
}