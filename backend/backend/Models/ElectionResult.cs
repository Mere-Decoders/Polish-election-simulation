using System.Text.Json;

namespace backend.Data;


public class ElectionResult
{
    public string[] PartyNames { get; set; }
    public Dictionary<string, int[]> DistrictResults { get; set; }

    public ElectionResult(string[] partyNames, Dictionary<string, int[]> districtResults)
    {
        PartyNames = partyNames;
        DistrictResults = districtResults;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }
}

