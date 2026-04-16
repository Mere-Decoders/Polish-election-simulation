using backend.Data;
using backend.Models;
using MoonSharp.Interpreter;

namespace backend.Services.Lua;

public interface ILuaScriptRunner
{
    public DynValue RunLuaCode(string code, SimulationData data, DistrictDetails districtDetails);
}