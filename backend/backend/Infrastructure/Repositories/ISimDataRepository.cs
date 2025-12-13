using backend.Data;

namespace backend.Infrastructure.Repositories;

public interface ISimDataRepository
{
    Task<SimulationData?> GetAsync(Guid id);
    Task<IEnumerable<SimulationData>> GetAllAsync();
    Task<SimulationData> AddAsync(Guid guid, SimulationData data);
    Task UpdateAsync(Guid id, SimulationData data);
    Task DeleteAsync(Guid id);
}