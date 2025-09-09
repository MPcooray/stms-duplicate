using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stms.Api.Dtos;
using Stms.Api.Services;

namespace Stms.Api.Controllers;

[ApiController]
[Route("api/universities/{universityId:int}/players")]
[Authorize]
public class PlayersController : ControllerBase
{
    private readonly IPlayerService _svc;
    public PlayersController(IPlayerService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> List(int universityId, CancellationToken ct)
        => Ok(await _svc.ListAsync(universityId, ct));

    [HttpPost]
    public async Task<IActionResult> Create(int universityId, PlayerCreateDto dto, CancellationToken ct)
    {
        var p = await _svc.CreateAsync(universityId, dto, ct);
        return CreatedAtAction(nameof(List), new { universityId }, p);
    }
}

