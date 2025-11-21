using backend.Data;

namespace backend.Services.Methods
{
    public class DhondtMethod
    {
        public ElectionResult Run(SimulationData data) {

            var partyNames = data.Parties.Select(p => p.Name).ToArray();
            var districtResults = new Dictionary<string, int[]>();

            foreach (var district in data.Districts)
            {
                string districtName = district.Key;
                string[] areaNames = district.Value;

                // Sumujemy głosy z obszarów w tym okręgu
                var districtVotes = SumVotesForDistrict(areaNames, data);

                // Filtr według progów
                var filteredVotes = ApplyThresholds(districtVotes, data.Parties);

                // Liczymy mandaty
                int mandates = SumMandatesFromAreas(areaNames, data);

                int[] seats = CalculateDhondt(filteredVotes, mandates);

                districtResults[districtName] = seats;
            }

            return new ElectionResult(partyNames, districtResults);
        }

        private Dictionary<string, int> SumVotesForDistrict(string[] areaNames, SimulationData data)
        {
            var totalVotes = new Dictionary<string, int>();

            foreach (var party in data.Parties)
                totalVotes[party.Name] = 0;

            foreach (var area in areaNames)
            {
                var (mandates, votes) = data.MandateNumberAndVotesInAreas[area];

                int partyIndex = 0;
                foreach (var party in data.Parties)
                {
                    totalVotes[party.Name] += votes[partyIndex];
                    partyIndex++;
                }
            }

            return totalVotes;
        }

        private int SumMandatesFromAreas(string[] areaNames, SimulationData data)
        {
            int total = 0;

            foreach (var area in areaNames)
            {
                var (mandates, _) = data.MandateNumberAndVotesInAreas[area];
                total += mandates;
            }

            return total;
        }

        private Dictionary<string, int> ApplyThresholds(
            Dictionary<string, int> votes,
            Party[] parties)
        {
            int totalVotes = votes.Values.Sum();

            var result = new Dictionary<string, int>();

            int index = 0;
            foreach (var party in parties)
            {
                int v = votes[party.Name];

                bool pass;
                if (!party.NeedsThreshold)
                {
                    pass = true; // brak progu
                }
                else if (party.IsCoalition)
                {
                    pass = (v >= 0.08 * totalVotes);
                }
                else
                {
                    pass = (v >= 0.05 * totalVotes);
                }

                result[party.Name] = pass ? v : 0;

                index++;
            }

            return result;
        }

        private int[] CalculateDhondt(Dictionary<string, int> votes, int mandates)
        {
            var parties = votes.Keys.ToList();
            var seatCount = parties.ToDictionary(p => p, p => 0);

            var quotients = new List<(string party, double quotient)>();

            foreach (var p in parties)
            {
                for (int i = 1; i <= mandates; i++)
                {
                    quotients.Add((p, (double)votes[p] / i));
                }
            }

            var top = quotients
                .OrderByDescending(q => q.quotient)
                .Take(mandates)
                .ToList();

            foreach (var q in top)
                seatCount[q.party]++;

            // Zwracamy tablicę w kolejności oryginalnych partii
            return parties.Select(p => seatCount[p]).ToArray();

        }
    }
}
