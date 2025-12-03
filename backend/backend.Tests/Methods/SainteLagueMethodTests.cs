using backend.Services.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Tests.Methods
{
    public class SainteLagueMethodTests
    {
        [Fact]
        public void Case1_Runs()
        {
            var result = SainteLagueMethod.RunSainteLague(SainteLagueData.Case1);
            Assert.NotNull(result);
            string[] names = { "Party A", "Party B", "Party C", "Party D" };
            Assert.Equal(names, result.PartyNames);
            Dictionary<string, int[]> expectedDistrictResults = new Dictionary<string, int[]>
        {
            { "1", new int[] { 3, 3, 1, 1 } },
            
        };
            Assert.Equal(expectedDistrictResults.Keys, result.ConstituencySeats.Keys);
            Assert.Equal(expectedDistrictResults["1"], result.ConstituencySeats["1"]);
            Dictionary<string, int[]> expectedVoteNumbers = new Dictionary<string, int[]>
        {
            { "1", new[] { 100_000, 80_000, 30_000, 20_000 } }
        };
            Assert.Equal(expectedVoteNumbers.Keys, result.ConstituencyVotes.Keys);
            Assert.Equal(expectedVoteNumbers["1"], result.ConstituencyVotes["1"]);
        }

    }
}
