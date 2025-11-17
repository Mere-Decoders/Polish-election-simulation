using backend.Data;

namespace backend.Services;

public interface ISimContextManager
{
    public SimContext FetchContextByGuid(Guid simDataGuid, Guid methodGuid);
}