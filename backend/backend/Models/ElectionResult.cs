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
        var partyList = string.Join(", ", PartyNames.Select(p => $"\"{p}\""));

        var districts = DistrictResults
            .Select(d =>
                $"\"{d.Key}\": [{string.Join(", ", d.Value)}]"
            );

        string districtJson = string.Join(",\n    ", districts);

        return
            $@"{{
              ""PartyNames"": [{partyList}],
              ""DistrictResults"": {{
                {districtJson}
              }}
            }}";
    }
}
