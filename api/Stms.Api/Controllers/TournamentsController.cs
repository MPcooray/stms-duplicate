using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stms.Api.Models;
using Stms.Api.Services;

namespace Stms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TournamentsController : ControllerBase
{
    private readonly ITournamentService _svc;
    public TournamentsController(ITournamentService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tournament>>> GetAll(CancellationToken ct)
        => Ok(await _svc.GetAllAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Tournament>> GetById(int id, CancellationToken ct)
    {
        var t = await _svc.GetByIdAsync(id, ct);
        return t is null ? NotFound() : Ok(t);
    }

    [HttpPost]
    public async Task<ActionResult<Tournament>> Create([FromBody] Tournament t, CancellationToken ct)
    {
        try
        {
            var created = await _svc.CreateAsync(t, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Tournament t, CancellationToken ct)
    {
        try
        {
            var ok = await _svc.UpdateAsync(id, t, ct);
            return ok ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var ok = await _svc.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
