using System.Text;
using backend.Data;
using backend.Models;
using backend.Services.Methods;

namespace backend.Services.Lua;
using MoonSharp.Interpreter;

public class LuaService : IMethodService
{
    private static string dhondtMethod = "";
    private static string highStakesMethod = @"
local winningParty = 1
local maxVotes = DistrictPartyVotes(1)
for i = 2,TotalParties() do
    if DistrictPartyVotes(i) > maxVotes then
        maxVotes = DistrictPartyVotes(i)
        winningParty = i
    end
end
local t = {}
for i = 1, TotalParties() do
    t[i] = (i == winningParty) and DistrictSeats() or 0
end
return t
";
    private static string sainteLagueMethod = "";

    private ILuaScriptRunner _scriptRunner;

    public LuaService()
    {
        _scriptRunner = new LuaScriptRunner();
    }
    
    private LuaResult Execute(string code, SimulationData data)
    {
        var partyNames = data.Parties.Select(p => p.Name).ToArray();
        var constituencyVotes = new Dictionary<string, int[]>();
        var constituencySeats = new Dictionary<string, int[]>();
        _scriptRunner.Code = code;
        _scriptRunner.SimulationData = data;

        try
        {
            foreach (var district in data.Districts)
            {
                var votes = new int[partyNames.Length];
                foreach (var area in district.Value.TerytCodes)
                {
                    for (var i = 0; i < partyNames.Length; i++)
                    {
                        votes[i] += data.VotesInAreas[area][i];
                    }
                }
                constituencyVotes.Add(district.Key, votes);

                var seats = new int[partyNames.Length];
                var result = _scriptRunner.RunLuaCode(district.Value);
                if (result.Type != DataType.Table)
                    throw new Exception("Script must return a table.");
                var table = result.Table;
                if (table.Length != partyNames.Length)
                    throw new Exception($"Table must have exactly {partyNames.Length} elements, it had {table.Length}.");
                for (var i = 1; i <= table.Length; i++)
                {
                    var val = table.Get(i);
                    if (val.Type != DataType.Number)
                        throw new Exception($"Element [{i}] must be an integer, was of the type {val.Type}.");
                    var doubleVal = val.Number;
                    if (doubleVal != Math.Floor(doubleVal))
                        throw new Exception($"Element [{i}] must be an integer, had value {doubleVal}.");
                    seats[i - 1] = (int)doubleVal;
                }
                constituencySeats.Add(district.Key, seats);
            }
            return new LuaResult(true,
                new ElectionResult(partyNames, constituencyVotes, constituencySeats),
                null);
        }
        catch (Exception ex)
        {
            return new LuaResult(false, null, ex.Message);
        }
    }

    public Func<SimulationData, ElectionResult> GetMethodByGuid(Guid methodGuid)
    {
        switch (methodGuid.ToString())
        {
            case ("00000000-0000-0000-0000-000000000000"):
                return (data => Execute(dhondtMethod, data).Output );
            case ("00000000-0000-0000-0000-000000000001"):
                return (data => Execute(highStakesMethod, data).Output );
            case ("00000000-0000-0000-0000-000000000002"):
                return (data => Execute(sainteLagueMethod, data).Output);
            default:
                throw new ArgumentException("The method you are requesting doesn't exist");
        }
    }
}