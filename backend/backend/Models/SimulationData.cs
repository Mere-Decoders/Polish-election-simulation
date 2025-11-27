namespace backend.Data;

public class SimulationData(Party[] parties, Dictionary<string, (int, string[])> districts,
    Dictionary<string, int[]> votesInAreas)
{
    public Party[] Parties { get; set; } = parties;

    public Dictionary<string, (int, string[])> Districts { get; set; } = districts;

    public Dictionary<string, int[]> VotesInAreas { get; set; } = votesInAreas;
}