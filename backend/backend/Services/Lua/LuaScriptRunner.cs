using backend.Data;
using backend.Models;
using MoonSharp.Interpreter;

namespace backend.Services.Lua;

public class LuaScriptRunner : ILuaScriptRunner
{
    private readonly Script _script = new Script(CoreModules.Preset_SoftSandbox);
    private DistrictDetails? _districtDetails;
    public SimulationData SimulationData { get; set; }
    public string? Code { get; set; }

    /// <summary>
    /// Sets up all the Lua methods.
    /// </summary>
    public LuaScriptRunner()
    {
        _script.Globals["IsCoalition"] = (Func<int, bool>)((i) => SimulationData.Parties[i - 1].IsCoalition);
        _script.Globals["NeedsThreshold"] = (Func<int, bool>)((i) => SimulationData.Parties[i - 1].NeedsThreshold);
        _script.Globals["TotalParties"] = (Func<int>) (() => SimulationData.Parties.Length);
        _script.Globals["DistrictSeats"] = (Func<int>) (() => _districtDetails.Seats);
        _script.Globals["DistrictPartyVotes"] = (Func<int, int>)(
            (i) => _districtDetails.TerytCodes.Sum(area => SimulationData.VotesInAreas[area][i - 1])
            );
        _script.Globals["NationalPartyVotes"] = (Func<int, int>)(
            (i) => SimulationData.VotesInAreas.Values.Sum(arr => arr[i - 1])
            );
        _script.Globals["DistrictTotalVotes"] = (Func<int>) (() =>
            _districtDetails.TerytCodes.Sum(area => SimulationData.VotesInAreas[area].Sum(i => i)));
        _script.Globals["NationalTotalVotes"] = (Func<int>) (() =>
            SimulationData.VotesInAreas.Values.Sum(arr => arr.Sum(i => i)));
    }
    
    public DynValue RunLuaCode(DistrictDetails districtDetails)
    {
        _districtDetails = districtDetails;
        return _script.DoString(Code);
    }
}