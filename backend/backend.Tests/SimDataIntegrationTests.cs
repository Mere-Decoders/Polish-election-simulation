using System.Text.Json;
using backend.Infrastructure;
using backend.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.Tests;

using backend.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class SimDataIntegrationTests
{
    [Fact]
    public async Task CanInsertSimulationData()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: false)
            .Build();

        var connStr = config.GetConnectionString("DefaultConnection");
        
        var services = new ServiceCollection();
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connStr));

        services.AddScoped<ISimDataRepository, SimDataRepository>();
        var provider = services.BuildServiceProvider();

        var repo = provider.GetRequiredService<ISimDataRepository>();

        var simData = SimulationDataFromCsv.Get();
        var guid = Guid.Empty;
        await repo.AddAsync(guid, simData); //this line will now fail because the object is already in db

        var inserted = (await repo.GetAsync(guid));
        var insertedJson = JsonSerializer.Serialize(inserted);
        var expectedJson = JsonSerializer.Serialize(simData);
        Assert.Equal(expectedJson, insertedJson);

    }
}
