namespace Stms.Api.Models;
public class Event {
  public int Id { get; set; }
  public string Code { get; set; } = null!;
  public string Name { get; set; } = null!;
  public int Distance { get; set; }
  public string Stroke { get; set; } = null!;
}
