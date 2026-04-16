using backend.Data;
using backend.Models;
using MoonSharp.Interpreter;

namespace backend.Services.Lua;

public interface ILuaScriptRunner
{
    SimulationData SimulationData { get; set; }
    string Code { get; set; }
    public DynValue RunLuaCode(DistrictDetails districtDetails);
}