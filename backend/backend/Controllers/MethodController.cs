using backend.Data;
using backend.Infrastructure.Repositories;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;


[Authorize]
[ApiController]
[Route("api/methods/[controller]")]
public class MethodController : ControllerBase
{
    private readonly ISimMethodRepository _repo;

    public MethodController(ISimMethodRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("get-list")]
    public async Task<ActionResult<IEnumerable<Method>>> GetMethodList()
    {
        var methods = await _repo.GetAllAsync();
        return Ok(methods);
    }

    [HttpGet("details")]
    public async Task<ActionResult<Method>> GetMethod(Guid guid)
    {
        var method = await _repo.GetAsync(guid);
        if (method == null)
            return NotFound();
        return Ok(method);
    }

    [HttpPost("add")]
    public async Task<ActionResult<Method>> AddMethod([FromBody] Method request)
    {
        var method = await _repo.AddAsync(Guid.NewGuid(), request.Name!, request.LuaCode);
        return CreatedAtAction(nameof(GetMethod), new { guid = method.Id }, method);
    }

    [HttpPut("update/{id:guid}")]
    public async Task<IActionResult> UpdateMethod(Guid id, [FromBody] Method request)
    {
        try
        {
            await _repo.UpdateAsync(id, request.Name!, request.LuaCode);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteMethod(Guid id)
    {
        var method = await _repo.GetAsync(id);
        if (method == null)
            return NotFound();

        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
