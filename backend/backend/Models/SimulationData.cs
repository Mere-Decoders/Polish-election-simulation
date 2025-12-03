using backend.Models;

namespace backend.Data;

public class SimulationData(Party[] parties, Dictionary<string, DistrictDetails> districts,
    Dictionary<string, int[]> votesInAreas)
{
    public Party[] Parties { get; set; } = parties;

    public Dictionary<string, DistrictDetails> Districts { get; set; } = districts;

    public Dictionary<string, int[]> VotesInAreas { get; set; } = votesInAreas;
}