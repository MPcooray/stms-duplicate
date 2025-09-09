using Stms.Api.Dtos;
using Stms.Api.Models;

namespace Stms.Api.Services;

public interface IPlayerService
{
    Task<List<Player>> ListAsync(int universityId, CancellationToken ct = default);
    Task<Player> CreateAsync(int universityId, PlayerCreateDto dto, CancellationToken ct = default);
}
