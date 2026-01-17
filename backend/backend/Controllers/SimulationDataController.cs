using backend.Data;
using backend.Services;
using backend.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Authorize]
[ApiController]
[Route("api/sim-data/[controller]")]
public class SimulationDataController : ControllerBase
{
    private readonly ISimDataService _simDataService;
    private readonly ICurrentUser _currentUser;
    public SimulationDataController(ISimDataService simDataService, ICurrentUser currentUser)
    {
        _simDataService = simDataService;
        _currentUser = currentUser;
    }
    
    [HttpGet("get-list-old")]
    public  ActionResult< List<(String, Guid)>> GetSimDataListOld()
    {
        var options = new Dictionary<String, Guid>();
        options.Add("wybory 2023 [przetworzone]", Guid.Empty);
        options.Add("wiarygodne wybory", Guid.Parse("00000000-0000-0000-0000-000000000001"));
        return Ok(options);
    }

    [HttpGet("get-list")]
    public async Task<IActionResult> GetSimDataList()
    {
        Guid guid = _currentUser.Value.Id;
        var datas = await _simDataService.GetUsersSimData(guid);
        return Ok(datas.Select(r => new { name = r.Label, id = r.DataId }));
}
    
    [HttpGet("details/{guid:guid}")]
    public async Task<ActionResult<SimulationData>> GetSimData(Guid guid)
    {
        var simData = await _simDataService.GetSimDataByGuid(guid);
        return Ok(simData);
    }
}