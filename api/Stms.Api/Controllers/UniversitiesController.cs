using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stms.Api.Models;
using Stms.Api.Services;

namespace Stms.Api.Controllers;

[ApiController]
[Route("api/tournaments/{tournamentId:int}/[controller]")]
[Authorize]
public class UniversitiesController : ControllerBase
{
    private readonly IUniversityService _svc;
    public UniversitiesController(IUniversityService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<University>>> List(int tournamentId, CancellationToken ct)
        => Ok(await _svc.ListAsync(tournamentId, ct));

    [HttpPost]
    public async Task<ActionResult<University>> Create(int tournamentId, [FromBody] University u, CancellationToken ct)
    {
        try
        {
            var created = await _svc.CreateAsync(tournamentId, u, ct);
            return CreatedAtAction(nameof(List), new { tournamentId }, created);
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int tournamentId, int id, [FromBody] University u, CancellationToken ct)
    {
        try
        {
            var ok = await _svc.UpdateAsync(tournamentId, id, u, ct);
            return ok ? NoContent() : NotFound();
        }
        catch (ArgumentException ex) { return BadRequest(ex.Message); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int tournamentId, int id, CancellationToken ct)
    {
        var ok = await _svc.DeleteAsync(tournamentId, id, ct);
        return ok ? NoContent() : NotFound();
    }
}
