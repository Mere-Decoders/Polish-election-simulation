using backend.Data;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Authorize]
[ApiController]
[Route("api/sim-data/[controller]")]
public class SimulationDataController : ControllerBase
{
    private readonly ISimDataService _simDataService;
    public SimulationDataController(ISimDataService simDataService)
    {
        _simDataService = simDataService;
    }
    
    [HttpGet("get-list")]
    public  ActionResult< List<(String, Guid)>> GetSimDataList()
    {
        var options = new Dictionary<String, Guid>();
        options.Add("wybory 2023 [przetworzone]", Guid.Empty);
        options.Add("wiarygodne wybory", Guid.Parse("00000000-0000-0000-0000-000000000001"));
        return Ok(options);
    }
    
    [HttpGet("details/{guid:guid}")]
    public async Task<ActionResult<SimulationData>> GetSimData(Guid guid)
    {
        var simData = await _simDataService.GetSimDataByGuid(guid);
        return Ok(simData);
    }
}