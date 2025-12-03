using backend.Data;
using backend.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;


[Authorize]
[ApiController]
[Route("api/methods/[controller]")]
public class MethodController : ControllerBase
{
    [HttpGet("get-list")]
    public ActionResult< List<(Guid, String)>> GetMethodList()
    {
        return Ok(new NotImplementedException());
    }
    
    [HttpGet("details")]
    public ActionResult< MethodDto> GetMethod(Guid guid)
    {
        return Ok(new NotImplementedException());
    }
}