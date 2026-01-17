using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;


[Authorize]
[ApiController]
[Route("api/methods/[controller]")]
public class MethodController : ControllerBase
{
    [HttpGet("get-list")]
    public ActionResult< Dictionary<String, Guid>> GetMethodList()
    {
        var options = new[]
        {
            new { name = "Metoda Dhondt'a", id = Guid.Empty },
            new { name = "Metoda High Stakes", id = Guid.Parse("00000000-0000-0000-0000-000000000001") },
            new { name = "Metoda Sainte-Lague", id = Guid.Parse("00000000-0000-0000-0000-000000000002") }
        };

        return Ok(options);
    }
    
    [HttpGet("details")]
    public ActionResult GetMethod(Guid guid)
    {
        return Ok(new NotImplementedException());
    }
}