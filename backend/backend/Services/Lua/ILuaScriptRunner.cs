using backend.Data;
using backend.Models;
using MoonSharp.Interpreter;

namespace backend.Services.Lua;

/// <summary>
/// Runs code using simulation data for a particular district.
/// </summary>
public interface ILuaScriptRunner
{
    SimulationData SimulationData { get; set; }
    string Code { get; set; }
    /// <summary>
    /// Runs <c>Code</c> using <c>SimulationData</c> for the particular district.
    /// </summary>
    /// <param name="districtDetails">District for which the seats are calculated.</param>
    /// <returns></returns>
    public DynValue RunLuaCode(DistrictDetails districtDetails);
}