using Microsoft.AspNetCore.Mvc;
using Stms.Api.Services;
using Stms.Api.Dtos;

namespace Stms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly IResultService _svc;      // <-- interface
    private readonly IRankingService _rank;

    public ResultsController(IResultService svc, IRankingService rank) // <-- interface
    {
        _svc  = svc;
        _rank = rank;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ResultCreateDto dto, CancellationToken ct)
    {
        var res = await _svc.CreateAsync(dto.PlayerId, dto.EventId, dto.TimingMs, dto.Heat, dto.Lane, ct);
        await _rank.RecalculateForEvent(dto.EventId, ct);
        return Ok(res);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ResultUpdateDto dto, CancellationToken ct)
    {
        var res = await _svc.UpdateAsync(id, dto.TimingMs, dto.Heat, dto.Lane, ct);
        await _rank.RecalculateForEvent(res.EventId, ct);
        return Ok(res);
    }
}
