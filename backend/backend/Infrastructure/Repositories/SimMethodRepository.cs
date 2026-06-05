using backend.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Infrastructure.Repositories
{
    public class SimMethodRepository : ISimMethodRepository
    {
        private readonly AppDbContext _db;

        public SimMethodRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Method?> GetAsync(Guid id)
        {
            return await _db.SimulationMethodsStore
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Select(m => new Method
                {
                    Id = m.Id,
                    Name = m.Name,
                    LuaCode = m.LuaCode
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Method>> GetAllAsync()
        {
            return await _db.SimulationMethodsStore
                .AsNoTracking()
                .Select(m => new Method
                {
                    Id = m.Id,
                    Name = m.Name,
                    LuaCode = m.LuaCode
                })
                .ToListAsync();
        }

        public async Task<Method> AddAsync(Guid guid, string name, string luaCode)
        {
            var entity = new SimulationMethodEntity
            {
                Id = guid,
                Name = name,
                LuaCode = luaCode
            };
            _db.SimulationMethodsStore.Add(entity);
            await _db.SaveChangesAsync();
            return new Method
            {
                Id = entity.Id,
                Name = entity.Name,
                LuaCode = entity.LuaCode
            };
        }

        public async Task UpdateAsync(Guid id, string name, string luaCode)
        {
            var entity = await _db.SimulationMethodsStore.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Method with id {id} not found.");
            }
            entity.Name = name;
            entity.LuaCode = luaCode;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.SimulationMethodsStore.FindAsync(id);
            if (entity == null) return;
            _db.SimulationMethodsStore.Remove(entity);
            await _db.SaveChangesAsync();

        }
    }
}
