namespace Stms.Api.Models;
public class Tournament {
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Venue { get; set; }
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public string Status { get; set; } = "Planned";
  public List<University> Universities { get; set; } = new();
}
