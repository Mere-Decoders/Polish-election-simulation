using backend.Data;

namespace backend.Services.Methods;

public class HighStakesMethod
{
    public static ElectionResult RunHighStakes(SimulationData data, double coalitionThreshold = 0.08, double lowerThreshold = 0.05)
        {
            string[] partyNames = data.Parties.Select(p => p.Name).ToArray();
            int partyCount = data.Parties.Length;

            var districtResults = new Dictionary<string, int[]>();
            var voteNumbers = new Dictionary<string, int[]>();

          //todo

            return new ElectionResult(partyNames, districtResults, voteNumbers);
        }
}