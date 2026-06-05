using backend.Data;
using backend.Models;
using backend.Services.Methods;
using System.Text;

namespace backend.Services.Lua;

using backend.Infrastructure.Repositories;
using MoonSharp.Interpreter;

public class LuaService : IMethodService
{
    private const string DhondtMethod = @"
local coalitionThreshold = 0.08
local lowerThreshold = 0.05
local passesThreshold = {}
for i = 1, TotalParties() do
    if not NeedsThreshold(i) then
        passesThreshold[i] = true
    else
        local percentage = NationalPartyVotes(i) / NationalTotalVotes()
        if IsCoalition(i) then
            passesThreshold[i] = percentage >= coalitionThreshold
        else
            passesThreshold[i] = percentage >= lowerThreshold
        end
    end
end

local quotients = {}
for i = 1, TotalParties() do
    if passesThreshold[i] then
        for d = 1, DistrictSeats() do
            table.insert(quotients, { party = i, quotient = DistrictPartyVotes(i) / d })
        end
    end
end

local function compareQuotients(a, b)
    return a.quotient > b.quotient
end

table.sort(quotients, compareQuotients)
local t = {}
for i = 1, TotalParties() do
    t[i] = 0
end
for i = 1, DistrictSeats() do
    local index = quotients[i].party
    t[index] = t[index] + 1
end

return t
";
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
    private const string SainteLagueMethod = @"
local coalitionThreshold = 0.08
local lowerThreshold = 0.05
local passesThreshold = {}
for i = 1, TotalParties() do
    if not NeedsThreshold(i) then
        passesThreshold[i] = true
    else
        local percentage = NationalPartyVotes(i) / NationalTotalVotes()
        if IsCoalition(i) then
            passesThreshold[i] = percentage >= coalitionThreshold
        else
            passesThreshold[i] = percentage >= lowerThreshold
        end
    end
end

local quotients = {}
for i = 1, TotalParties() do
    if passesThreshold[i] then
        for d = 0, DistrictSeats() - 1 do
            table.insert(quotients, { party = i, quotient = DistrictPartyVotes(i) / (d + 0.5) })
        end
    end
end

local function compareQuotients(a, b)
    return a.quotient > b.quotient
end

table.sort(quotients, compareQuotients)
local t = {}
for i = 1, TotalParties() do
    t[i] = 0
end
for i = 1, DistrictSeats() do
    local index = quotients[i].party
    t[index] = t[index] + 1
end

return t
";

    private ILuaScriptRunner _scriptRunner;
    private readonly ISimMethodRepository _methodRepository;


    public LuaService(ISimMethodRepository methodRepository)
    {
        _scriptRunner = new LuaScriptRunner();
        _methodRepository = methodRepository;
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
                new ElectionResult(partyNames, constituencySeats, constituencyVotes),
                null);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
        var method = _methodRepository
            .GetAsync(methodGuid)
            .GetAwaiter()
            .GetResult();

        if (method == null)
            throw new ArgumentException("Method not found");

        return data => Execute(method.LuaCode, data).Output;
    }
}