namespace backend.Data;

public class SimulationData
{
    public Party[] Parties { get; set; }

    public  Dictionary<string, string[]> Districts { get; set; }
    
    public Dictionary<string, int[]> VotesInAreas { get; set; }

    public SimulationData(Party[] parties, Dictionary<string, string[]> districts,
        Dictionary<string, int[]> votesInAreas)
    {
        Parties = parties;
        Districts = districts;
        VotesInAreas = votesInAreas;
    }
}