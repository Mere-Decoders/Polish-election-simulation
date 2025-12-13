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
        var options = new Dictionary<String, Guid>();
        options.Add("Metoda Dhondt'a", Guid.Empty);
        options.Add("Metoda High Stakes", Guid.Parse("00000000-0000-0000-0000-000000000001"));
        options.Add("Metoda Sainte-Lague", Guid.Parse("00000000-0000-0000-0000-000000000002"));
        return Ok(options);
    }
    
    [HttpGet("details")]
    public ActionResult GetMethod(Guid guid)
    {
        return Ok(new NotImplementedException());
    }
}