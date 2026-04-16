using System.Text;
using backend.Data;
using backend.Models;
using backend.Services.Methods;

namespace backend.Services.Lua;
using MoonSharp.Interpreter;

public class LuaService : IMethodService
{
    //TODO
    private const string DhondtMethod = "";
    private const string HighStakesMethod = @"
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
    //TODO
    private const string SainteLagueMethod = "";

    private ILuaScriptRunner _scriptRunner;

    public LuaService()
    {
        _scriptRunner = new LuaScriptRunner();
    }
    
    /// <summary>
    /// Runs <c>code</c> 
    /// </summary>
    /// <param name="code">Script to be run.</param>
    /// <param name="data">Simulation data for the script.</param>
    /// <returns>Result of the script, containing either <c>ElectionResult</c> or an error message.</returns>
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
                var result = _scriptRunner.RunLuaCode(district.Value);
                var seats = LuaTableToArray(partyNames.Length, result);
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

    /// <summary>
    /// Converts a Lua value <c>result</c> into a C# <c>int[]</c>.
    /// Throws <c>Exception</c> if the result is not a table of integers of the right length.
    /// </summary>
    /// <param name="intCount">Expected table length.</param>
    /// <param name="result">Result of Lua program.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static int[] LuaTableToArray(int intCount, DynValue result)
    {
        var array = new int[intCount];
        if (result.Type != DataType.Table)
            throw new Exception("Script must return a table.");
        var table = result.Table;
        if (table.Length != intCount)
            throw new Exception($"Table must have exactly {intCount} elements, it had {table.Length}.");
        for (var i = 1; i <= table.Length; i++)
        {
            var val = table.Get(i);
            if (val.Type != DataType.Number)
                throw new Exception($"Element [{i}] must be an integer, was of the type {val.Type}.");
            var doubleVal = val.Number;
            if (doubleVal != Math.Floor(doubleVal))
                throw new Exception($"Element [{i}] must be an integer, had value {doubleVal}.");
            array[i - 1] = (int)doubleVal;
        }

        return array;
    }

    public Func<SimulationData, ElectionResult> GetMethodByGuid(Guid methodGuid)
    {
        switch (methodGuid.ToString())
        {
            case ("00000000-0000-0000-0000-000000000000"):
                return (data => Execute(DhondtMethod, data).Output );
            case ("00000000-0000-0000-0000-000000000001"):
                return (data => Execute(HighStakesMethod, data).Output );
            case ("00000000-0000-0000-0000-000000000002"):
                return (data => Execute(SainteLagueMethod, data).Output);
            default:
                throw new ArgumentException("The method you are requesting doesn't exist");
        }
    }
}