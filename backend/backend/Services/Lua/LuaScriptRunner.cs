using backend.Data;
using backend.Models;
using MoonSharp.Interpreter;

namespace backend.Services.Lua;

public class LuaScriptRunner : ILuaScriptRunner
{
    private Script _script = new Script(CoreModules.None);
    private int _totalVotes;
    private Party[] _parties;

    public LuaScriptRunner()
    {
        _script.Globals("IsCoalition") = (Func<bool>)((i) => )
    }
    
    public DynValue RunLuaCode(string code, SimulationData data, DistrictDetails districtDetails)
    {
        _parties = data.Parties;
        _totalVotes = data.
        throw new NotImplementedException();
    }
}