using backend.Data;

namespace backend.Services;

public interface ISimContextManager
{
    public SimContext FetchContext(Guid simDataGuid, Guid MethodGuid);
}