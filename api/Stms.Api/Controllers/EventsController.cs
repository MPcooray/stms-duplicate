using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stms.Api.Models;
using Stms.Api.Services;

namespace Stms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly IEventService _svc;
    public EventsController(IEventService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> List(CancellationToken ct)
        => Ok(await _svc.ListAsync(ct));
}
