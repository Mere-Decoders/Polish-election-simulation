namespace backend.Data;

public class SimulationData(Party[] parties, Dictionary<string, string[]> districts,
    Dictionary<string, (int, int[])> mandateNumberAndVotesInAreas)
{
    public Party[] Parties { get; set; } = parties;

    public Dictionary<string, string[]> Districts { get; set; } = districts;

    public Dictionary<string, (int, int[])> MandateNumberAndVotesInAreas { get; set; } = mandateNumberAndVotesInAreas;
}