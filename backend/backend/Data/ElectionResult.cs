namespace backend.Data;

public class ElectionResult
{
    public string[] PartyNames { get; set; }
    public Dictionary<String, int[]> DistrictResults { get; set; }

    public ElectionResult(string[] partyNames, Dictionary<String, int[]> districtResults)
     {
         PartyNames = partyNames;
         DistrictResults = districtResults;
     }
}