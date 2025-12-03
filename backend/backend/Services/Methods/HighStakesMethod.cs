using backend.Data;

namespace backend.Services.Methods;

public class HighStakesMethod
{
    public static ElectionResult RunHighStakes(SimulationData data, double coalitionThreshold = 0, double lowerThreshold = 0)
        {

        string[] partyNames = data.Parties.Select(p => p.Name).ToArray();

        //Sumowanie g³osów ogólnokrajowych
        
        int partyCount = data.Parties.Length;
        int[] nationalVotes = new int[partyCount];

        foreach (var area in data.VotesInAreas)
        {
            for (int i = 0; i < partyCount; i++)
                nationalVotes[i] += area.Value[i];
        }

        var districtResults = new Dictionary<string, int[]>();
        var voteNumbers = new Dictionary<string, int[]>();

        foreach (var district in data.Districts)
        {
            string districtId = district.Key;
            int seats = district.Value.Seats;
            string[] areas = district.Value.TerytCodes;

            // Suma g³osów per okrêg
            int[] districtVotes = new int[partyCount];

            foreach (var area in areas)
            {
                var areaVotes = data.VotesInAreas[area];
                for (int i = 0; i < partyCount; i++)
                    districtVotes[i] += areaVotes[i];
            }

            voteNumbers[districtId] = districtVotes;

            //Znajdowanie partii z najwiêksz¹ liczb¹ g³osów

            int winningParty = 0;
            int maxVotes = districtVotes[0];

            for (int i = 1; i < partyCount; i++)
            {
                if (districtVotes[i] > maxVotes)
                {
                    maxVotes = districtVotes[i];
                    winningParty = i;
                }
            }

            int[] seatResult = new int[partyCount];
            seatResult[winningParty] = seats;

            districtResults[districtId] = seatResult;
        }

        return new ElectionResult(partyNames, districtResults, voteNumbers);
    }
}