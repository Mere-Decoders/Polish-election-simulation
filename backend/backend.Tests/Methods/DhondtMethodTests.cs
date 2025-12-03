using backend.Services.Methods;
using Xunit.Abstractions;

namespace backend.Tests.Methods;

public class DhondtMethodTests
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
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case1);
        Assert.NotNull(result);
        string[] names = { "123", "456" }; 
        Assert.Equal( names , result.PartyNames );
        Dictionary<string, int[]> expectedDistrictResults = new Dictionary<string, int[]>
        {
            { "1", new int[] { 1, 6 } },
            { "2", new int[] { 4, 0 } }
        };
        Assert.Equal(expectedDistrictResults.Keys, result.ConstituencySeats.Keys);
        Assert.Equal(expectedDistrictResults["1"], result.ConstituencySeats["1"]);
        Assert.Equal(expectedDistrictResults["2"], result.ConstituencySeats["2"]);
        Dictionary<string, int[]> expectedVoteNumbers = new Dictionary<string, int[]>
        {
            { "1", new int[] { 30, 200 } },
            { "2", new int[] { 120, 0 } }
        };
        Assert.Equal(expectedVoteNumbers.Keys, result.ConstituencyVotes.Keys);
        Assert.Equal(expectedVoteNumbers["1"], result.ConstituencyVotes["1"]);
        Assert.Equal(expectedVoteNumbers["2"], result.ConstituencyVotes["2"]);
    }

    [Fact]
    public void Case2_Runs()
    {
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case2);
        Assert.NotNull(result);
        string[] names = { "A", "B" };
        Assert.Equal(names, result.PartyNames);
        Dictionary<string, int[]> expectedDistrictResults = new Dictionary<string, int[]>
        {
            { "1", new int[] { 0, 5 } }
        };
        Assert.Equal(expectedDistrictResults.Keys, result.ConstituencySeats.Keys);
        Assert.Equal(expectedDistrictResults["1"], result.ConstituencySeats["1"]);
        Dictionary<string, int[]> expectedVoteNumbers = new Dictionary<string, int[]>
        {
            { "1", new int[] { 50, 1000 } },
        };
        Assert.Equal(expectedVoteNumbers.Keys, result.ConstituencyVotes.Keys);
        Assert.Equal(expectedVoteNumbers["1"], result.ConstituencyVotes["1"]);
    }

    [Fact]
    public void Case3_Runs()
    {
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case3);
        Assert.NotNull(result);
        string[] names = { "A", "B" };
        Assert.Equal(names, result.PartyNames);
        Dictionary<string, int[]> expectedDistrictResults = new Dictionary<string, int[]>
        {
            { "1", new int[] { 0, 5 } }
        };
        Assert.Equal(expectedDistrictResults.Keys, result.ConstituencySeats.Keys);
        Assert.Equal(expectedDistrictResults["1"], result.ConstituencySeats["1"]);
        Dictionary<string, int[]> expectedVoteNumbers = new Dictionary<string, int[]>
        {
            { "1", new int[] { 50, 1000 } },
        };
        Assert.Equal(expectedVoteNumbers.Keys, result.ConstituencyVotes.Keys);
        Assert.Equal(expectedVoteNumbers["1"], result.ConstituencyVotes["1"]);
    }

    [Fact]
    public void Case4_Runs()
    {
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case4);
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
    public void Case5_Runs()
    {
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case5);
        Assert.NotNull(result);
        string[] names = { "A", "B" };
        Assert.Equal(names, result.PartyNames);
        Dictionary<string, int[]> expectedDistrictResults = new Dictionary<string, int[]>
        {
            { "1", new int[] { 3, 97 } }
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
    public void Case6_Runs()
    {
        var result = DhondtMethod.RunDhondt(DhondtTestData.Case6);
        Assert.NotNull(result);
        string[] names = { "123", "456" };
        Assert.Equal(names, result.PartyNames);
        Dictionary<string, int[]> expectedDistrictResults = new Dictionary<string, int[]>
        {
            { "1", new int[] { 3, 14 } },
            { "2", new int[] { 9, 5 } }
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