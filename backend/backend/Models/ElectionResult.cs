using System.Text.Json;

namespace backend.Data;


public class ElectionResult
{   
    public string[] PartyNames { get; set; }

    //Numer okregu i wyniki partii w tym okregu
    public Dictionary<string, int[]> ConstituencySeats { get; set; }

    //Numer okregu i suma glosow partii w tym okregu
    public Dictionary<string, int[]> ConstituencyVotes { get; set; }

    public ElectionResult(string[] partyNames, Dictionary<string, int[]> districtResults, 
        Dictionary<string, int[]> voteNumbers)
    {
        PartyNames = partyNames;
        ConstituencySeats = districtResults;
        ConstituencyVotes = voteNumbers;

    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}

