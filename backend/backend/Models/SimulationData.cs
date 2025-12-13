using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;
[Owned]
public class SimulationData
{
    private SimulationData() { }

    public SimulationData(
        Party[] parties,
        Dictionary<string, DistrictDetails> districts,
        Dictionary<string, int[]> votesInAreas)
    {
        Parties = parties;
        Districts = districts;
        VotesInAreas = votesInAreas;
    }

    public Party[] Parties { get; set; } = Array.Empty<Party>();

    public Dictionary<string, DistrictDetails> Districts { get; set; }
        = new();

    public Dictionary<string, int[]> VotesInAreas { get; set; }
        = new();
}