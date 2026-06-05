using backend.Models;
using backend.Services;
using backend.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Authorize]
[ApiController]
[Route("api/methods/[controller]")]
public class MethodController : ControllerBase
{
    private readonly ISimMethodService _simMethodService;
    private readonly ICurrentUser _currentUser;

    public MethodController(ISimMethodService simMethodService, ICurrentUser currentUser)
    {
        _simMethodService = simMethodService;
        _currentUser = currentUser;
    }

    [HttpGet("get-list")]
    public async Task<IActionResult> GetMethodList()
    {
        var claims = await _simMethodService.GetUserMethods(_currentUser.Value.Id);
        return Ok(claims.Select(r => new { name = r.Label, id = r.MethodId }));
    }

    [HttpGet("details/{guid:guid}")]
    public async Task<ActionResult<Method>> GetMethod(Guid guid)
    {
        try
        {
            var method = await _simMethodService.GetMethodByGuid(_currentUser.Value.Id, guid);
            return Ok(method);
        }
        catch (UnauthorizedAccessException e)
        {
            return Problem(e.Message, statusCode: 403);
        }
        catch (KeyNotFoundException e)
        {
            return Problem(e.Message, statusCode: 404);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateMethod([FromQuery] string label, [FromBody] Method data)
    {
        label = (label ?? string.Empty).Trim();
        if (label.Length == 0)
            return BadRequest("Label cannot be empty.");
        if (label.Length > 200)
            return BadRequest("Label is too long.");

        var claim = await _simMethodService.CreateMethodForUserAsync(
            _currentUser.Value.Id,
            label,
            data);

        return CreatedAtAction(
            nameof(GetMethod),
            new { guid = claim.MethodId },
            new { name = claim.Label, id = claim.MethodId });
    }

    [HttpPut("update/{guid:guid}")]
    public async Task<IActionResult> UpdateMethod(Guid guid, [FromBody] Method data)
    {
        try
        {
            await _simMethodService.UpdateMethodForUserAsync(_currentUser.Value.Id, guid, data);
            return NoContent();
        }
        catch (UnauthorizedAccessException e)
        {
            return Problem(e.Message, statusCode: 403);
        }
    }

    [HttpDelete("delete/{guid:guid}")]
    public async Task<IActionResult> DeleteMethod(Guid guid)
    {
        try
        {
            await _simMethodService.DeleteMethodForUserAsync(_currentUser.Value.Id, guid);
            return NoContent();
        }
        catch (UnauthorizedAccessException e)
        {
            return Problem(e.Message, statusCode: 403);
        }
    }
}