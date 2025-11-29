using backend.Data;
using backend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Authorize]
[ApiController]
[Route("api/sim-data/[controller]")]
public class SimulationDataController : ControllerBase
{
    [HttpGet("get-list")]
    public  ActionResult< DataResponse< List<(String, Guid)>>> GetSimDataList()
    {
        return Ok( new NotImplementedException());
    }
    
    [HttpGet("details")]
    public ActionResult<DataResponse< SimulationData>> GetSimData(Guid guid)
    {
        return Ok( new NotImplementedException());
    }
}