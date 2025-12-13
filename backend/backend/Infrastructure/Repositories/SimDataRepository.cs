using System.Text.Json;
using backend.Data;
using backend.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Repositories;

public class SimDataRepository : ISimDataRepository
{
    private readonly AppDbContext _db;

    public SimDataRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SimulationData?> GetAsync(Guid id)
    {
        var entity = await _db.SimulationDataStore.FindAsync(id);
        if (entity == null) return null;
        return JsonSerializer.Deserialize<SimulationData>(entity.Json);
    }

    public async Task<IEnumerable<SimulationData>> GetAllAsync()
    {
        var entities = await _db.SimulationDataStore.ToListAsync();
        return entities.Select(e => JsonSerializer.Deserialize<SimulationData>(e.Json)!)
            .ToList();
    }

    public async Task<SimulationData> AddAsync(Guid guid, SimulationData data)
    {
        var entity = new SimulationDataEntity
        {
            Id = guid,
            Json = JsonSerializer.Serialize(data)
        };

        _db.SimulationDataStore.Add(entity);
        await _db.SaveChangesAsync();
        return data;
    }

    public async Task UpdateAsync(Guid id, SimulationData data)
    {
        var entity = await _db.SimulationDataStore.FindAsync(id);
        if (entity == null) throw new KeyNotFoundException($"SimulationData {id} not found");

        entity.Json = JsonSerializer.Serialize(data);
        _db.SimulationDataStore.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.SimulationDataStore.FindAsync(id);
        if (entity == null) return;

        _db.SimulationDataStore.Remove(entity);
        await _db.SaveChangesAsync();
    }
}