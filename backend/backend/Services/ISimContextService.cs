using backend.Data;

namespace backend.Services;

public interface ISimContextService
{
    public Task<SimContext> FetchContextByGuid(Guid simDataGuid, Guid methodGuid);
}