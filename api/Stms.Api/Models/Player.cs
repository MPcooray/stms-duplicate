namespace Stms.Api.Models;

public class Player
{
    public int Id { get; set; }
    public int UniversityId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName  { get; set; } = default!;

    public University? University { get; set; }
}
