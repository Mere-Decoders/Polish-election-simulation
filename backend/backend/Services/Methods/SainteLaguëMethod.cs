using backend.Data;

namespace backend.Services.Methods
{
    public class SainteLaguëMethod
    {
        public static ElectionResult RunSainteLaguë(
            SimulationData data,                    
            double coalitionThreshold = 0.08,
            double lowerThreshold = 0.05,
            double divisor = 0.5  // classic: 0.5 || modified: != 0.5 -> Sweden: 0.6, Norway: 0.7
            )
        {
            string[] partyNames = data.Parties.Select(p => p.Name).ToArray();
            int partyCount = data.Parties.Length;

            // -------------------------
            // 1. Sumowanie głosów ogólnokrajowych
            // -------------------------
            int[] nationalVotes = new int[partyCount];

            foreach (var area in data.VotesInAreas)
            {
                for (int i = 0; i < partyCount; i++)
                    nationalVotes[i] += area.Value[i];
            }

            int totalVotes = nationalVotes.Sum();

            // -------------------------
            // 2. Sprawdzanie progów
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
            // 3. Podział mandatów w okręgach
            // -------------------------
            var districtResults = new Dictionary<string, int[]>();
            var voteNumbers = new Dictionary<string, int[]>();

            foreach (var district in data.Districts)
            {
                string districtId = district.Key;
                int seats = district.Value.Seats;
                string[] areas = district.Value.TerytCodes;

                int[] districtVotes = new int[partyCount];

                // Sumowanie głosów w okręgu
                foreach (var area in areas)
                {
                    var areaVotes = data.VotesInAreas[area];
                    for (int i = 0; i < partyCount; i++)
                        districtVotes[i] += areaVotes[i];
                }

                voteNumbers[districtId] = districtVotes;

                // -------------------------
                // Sainte-Laguë (modified): generowanie ilorazów
                // -------------------------
                var quotients = new List<(double value, int party)>();

                for (int i = 0; i < partyCount; i++)
                {
                    if (!passesThreshold[i])
                        continue;

                    for (int s = 0; s < seats; s++)
                    {
                        double div;

                        if (s == 0)
                        {
                            // modified divisor for FIRST quotient
                            div = divisor;  
                        }
                        else
                        {
                            // standard Sainte-Laguë divisors: 1.5, 2.5, 3.5...
                            div = s + 0.5;
                        }

                        double q = districtVotes[i] / div;

                        quotients.Add((q, i));
                    }
                }

                // sortujemy i bierzemy najlepsze N
                var top = quotients
                    .OrderByDescending(q => q.value)
                    .Take(seats)
                    .ToList();

                int[] seatResult = new int[partyCount];

                foreach (var t in top)
                    seatResult[t.party]++;

                districtResults[districtId] = seatResult;
            }

            return new ElectionResult(partyNames, districtResults, voteNumbers);
        }
    }
}

