namespace Stms.Api.Models;
public class University {
  public int Id { get; set; }
  public int TournamentId { get; set; }
  public string Name { get; set; } = null!;
  public Tournament? Tournament { get; set; }
  public List<Player> Players { get; set; } = new();
}
