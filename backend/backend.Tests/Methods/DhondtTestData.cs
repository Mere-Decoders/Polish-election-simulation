using backend.Data;
using backend.Models;

namespace backend.Tests.Methods;

public static class DhondtTestData
{
    public static readonly SimulationData Case1 = new SimulationData(
        new[]
        {
            new Party("123", false, false),
            new Party("456", false, true)
        },
        new Dictionary<string, DistrictDetails>
        {
            { "1", new DistrictDetails { Seats = 7, TerytCodes = new[] { "0052", "0721" } } },
            { "2", new DistrictDetails { Seats = 4, TerytCodes = new[] { "1021", "0823", "1304" } } }
        },
        new Dictionary<string, int[]>
        {
            { "0052", new[] { 10, 100 } },
            { "0721", new[] { 20, 100 } },
            { "1021", new[] { 30, 0 } },
            { "0823", new[] { 40, 0 } },
            { "1304", new[] { 50, 0 } }
        }
    );

    public static readonly SimulationData Case2 = new SimulationData(
        new[]
        {
            new Party("A", false, true),
            new Party("B", false, true)
        },
        new Dictionary<string, DistrictDetails>
        {
            { "1", new DistrictDetails { Seats = 5, TerytCodes = new[] { "001", "002" } } }
        },
        new Dictionary<string, int[]>
        {
            { "001", new[] { 50, 500 } },
            { "002", new[] { 0, 500 } }
        }
    );

    public static readonly SimulationData Case3 = new SimulationData(
        new[]
        {
            new Party("A", false, false),
            new Party("B", false, true)
        },
        new Dictionary<string, DistrictDetails>
        {
            { "1", new DistrictDetails { Seats = 5, TerytCodes = new[] { "001", "002" } } }
        },
        new Dictionary<string, int[]>
        {
            { "001", new[] { 50, 500 } },
            { "002", new[] { 0, 500 } }
        }
    );

    public static readonly SimulationData Case4 = new SimulationData(
        new[]
        {
            new Party("A", false, true),
            new Party("B", false, true)
        },
        new Dictionary<string, DistrictDetails>
        {
            { "1", new DistrictDetails { Seats = 100, TerytCodes = new[] { "001" } } }
        },
        new Dictionary<string, int[]>
        {
            { "001", new[] { 4, 100 } }
        }
    );

    public static readonly SimulationData Case5 = new SimulationData(
        new[]
        {
            new Party("A", false, false),
            new Party("B", false, true)
        },
        new Dictionary<string, DistrictDetails>
        {
            { "1", new DistrictDetails { Seats = 100, TerytCodes = new[] { "001" } } }
        },
        new Dictionary<string, int[]>
        {
            { "001", new[] { 4, 100 } }
        }
    );

    public static readonly SimulationData Case6 = new SimulationData(
        new[]
        {
            new Party("123", false, false),
            new Party("456", false, true)
        },
        new Dictionary<string, DistrictDetails>
        {
            { "1", new DistrictDetails { Seats = 17, TerytCodes = new[] { "0052", "0071" } } },
            { "2", new DistrictDetails { Seats = 14, TerytCodes = new[] { "1021", "0823", "1304" } } }
        },
        new Dictionary<string, int[]>
        {
            { "0052", new[] { 10, 100 } },
            { "0071", new[] { 30, 90 } },
            { "1021", new[] { 120, 50 } },
            { "0823", new[] { 33, 41 } },
            { "1304", new[] { 70, 24 } }
        }
    );
}
