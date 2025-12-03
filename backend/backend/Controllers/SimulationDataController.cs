using backend.Data;
using backend.DTOs;
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
        return Ok( new NotImplementedException());
    }
    
    [HttpGet("details/{guid:guid}")]
    public async Task<ActionResult<SimulationData>> GetSimData(Guid guid)
    {
        var simData = await _simDataService.GetSimDataByGuid(guid);
        return Ok(simData);
    }
}