using Microsoft.AspNetCore.Mvc;
using Stms.Api.Dtos;
using Stms.Api.Services;

namespace Stms.Api.Controllers;

[ApiController]
[Route("api/leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboard;

    public LeaderboardController(ILeaderboardService leaderboard)
        => _leaderboard = leaderboard;

    // Per-event leaderboard (ranked + time)
    [HttpGet("events/{eventId:int}")]
    public async Task<ActionResult<List<EventLeaderboardRow>>> GetEvent(int eventId, CancellationToken ct)
    {
        var rows = await _leaderboard.GetEventLeaderboard(eventId, ct);
        return Ok(rows);
    }

    // Tournament totals by university (sum of points)
    // Keep the route name you prefer; here I use /university-totals to be explicit.
    [HttpGet("tournaments/{tournamentId:int}/university-totals")]
    public async Task<ActionResult<List<UniversityTotalRow>>> GetTotals(int tournamentId, CancellationToken ct)
    {
        var rows = await _leaderboard.GetTournamentUniversityTotals(tournamentId, ct);
        return Ok(rows);
    }
}
