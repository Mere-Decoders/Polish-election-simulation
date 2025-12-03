using backend.Services.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Tests.Methods
{
    public class HighStakesTests
    {
        //Prosze nie usuwać tego komentarza, on jest tutaj celowo w celu debugowania testów.
        /* private readonly ITestOutputHelper _output;

         public DhondtMethodTests(ITestOutputHelper output)
         {
             _output = output;
         }*/

        [Fact]
        public void Case1_Runs()
        {
            var result = HighStakesMethod.RunHighStakes(HighStakesData.Case1);
            Assert.NotNull(result);
            string[] names = { "A", "B" };
            Assert.Equal(names, result.PartyNames);
            Dictionary<string, int[]> expectedDistrictResults = new Dictionary<string, int[]>
        {
            { "1", new int[] { 0, 100 } }
        };
            Assert.Equal(expectedDistrictResults.Keys, result.ConstituencySeats.Keys);
            Assert.Equal(expectedDistrictResults["1"], result.ConstituencySeats["1"]);
            Dictionary<string, int[]> expectedVoteNumbers = new Dictionary<string, int[]>
        {
            { "1", new int[] { 4, 100 } },
        };
            Assert.Equal(expectedVoteNumbers.Keys, result.ConstituencyVotes.Keys);
            Assert.Equal(expectedVoteNumbers["1"], result.ConstituencyVotes["1"]);
        }

        [Fact]
        public void Case2_Runs()
        {
            var result = HighStakesMethod.RunHighStakes(HighStakesData.Case2);
            Assert.NotNull(result);
            string[] names = { "123", "456" };
            Assert.Equal(names, result.PartyNames);
            Dictionary<string, int[]> expectedDistrictResults = new Dictionary<string, int[]>
        {
            { "1", new int[] { 0, 17 } },
            { "2", new int[] { 14, 0 } }
        };
            Assert.Equal(expectedDistrictResults.Keys, result.ConstituencySeats.Keys);
            Assert.Equal(expectedDistrictResults["1"], result.ConstituencySeats["1"]);
            Assert.Equal(expectedDistrictResults["2"], result.ConstituencySeats["2"]);
            Dictionary<string, int[]> expectedVoteNumbers = new Dictionary<string, int[]>
        {
            { "1", new int[] { 40, 190 } },
            { "2", new int[] { 223, 115 } }
        };
            Assert.Equal(expectedVoteNumbers.Keys, result.ConstituencyVotes.Keys);
            Assert.Equal(expectedVoteNumbers["1"], result.ConstituencyVotes["1"]);
            Assert.Equal(expectedVoteNumbers["2"], result.ConstituencyVotes["2"]);

        }


    }
}
