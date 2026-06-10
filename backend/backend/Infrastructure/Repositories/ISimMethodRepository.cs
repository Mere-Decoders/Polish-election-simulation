using backend.Models;

namespace backend.Infrastructure.Repositories

{
    public interface ISimMethodRepository
    {
        Task<Method?> GetAsync(Guid id);
        Task<IEnumerable<Method>> GetAllAsync();
        Task<Method> AddAsync(Guid guid, string name, string luaCode);
        Task UpdateAsync(Guid id, string name, string luaCode);
        Task DeleteAsync(Guid id);
    }
}
