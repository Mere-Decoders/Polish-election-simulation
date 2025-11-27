using backend.Data;

namespace backend.Services.Methods
{
    public class DhondtMethod
    {
        public static ElectionResult RunDhondt(SimulationData data, double coalitionThreshold = 0.08, double lowerThreshold = 0.05)
        {
            string[] partyNames = data.Parties.Select(p => p.Name).ToArray();

            // -------------------------
            // 1. Sumowanie głosów ogólnokrajowych
            // -------------------------
            int partyCount = data.Parties.Length;
            int[] nationalVotes = new int[partyCount];

            foreach (var area in data.VotesInAreas)
            {
                for (int i = 0; i < partyCount; i++)
                    nationalVotes[i] += area.Value[i];
            }

            int totalVotes = nationalVotes.Sum();

            // -------------------------
            // 2. Wyliczenie kto przekracza próg
            // -------------------------
            bool[] passesThreshold = new bool[partyCount];

            for (int i = 0; i < partyCount; i++)
            {
                var p = data.Parties[i];

                if (!p.NeedsThreshold)
                {
                    passesThreshold[i] = true;
                    continue;
                }

                double pct = (double)nationalVotes[i] / totalVotes;

                if (p.IsCoalition)
                    passesThreshold[i] = pct >= coalitionThreshold;
                else
                    passesThreshold[i] = pct >= lowerThreshold;
            }

            // -------------------------
            // 3. Liczenie mandatów w każdym okręgu
            // -------------------------
            var districtResults = new Dictionary<string, int[]>();

            foreach (var district in data.Districts)
            {
                string districtId = district.Key;
                int seats = district.Value.Item1;
                string[] areas = district.Value.Item2;

                // Suma głosów per okręg
                int[] districtVotes = new int[partyCount];

                foreach (var area in areas)
                {
                    var areaVotes = data.VotesInAreas[area];
                    for (int i = 0; i < partyCount; i++)
                        districtVotes[i] += areaVotes[i];
                }

                // d’Hondt: lista (value, partyIndex)
                var quotients = new List<(double value, int party)>();

                for (int i = 0; i < partyCount; i++)
                {
                    if (!passesThreshold[i])
                        continue; // nie przeszedł progu – zero dzielników

                    for (int d = 1; d <= seats; d++)
                    {
                        quotients.Add(((double)districtVotes[i] / d, i));
                    }
                }

                // wybieramy największe N
                var top = quotients
                    .OrderByDescending(q => q.value)
                    .Take(seats)
                    .ToList();

                int[] seatResult = new int[partyCount];
                foreach (var t in top)
                    seatResult[t.party]++;

                districtResults[districtId] = seatResult;
            }

            return new ElectionResult(partyNames, districtResults);
        }
    }    
}
