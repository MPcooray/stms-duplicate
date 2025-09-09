namespace Stms.Api.Dtos;

public class ResultCreateDto
{
    public int PlayerId { get; set; }
    public int EventId  { get; set; }
    public int TimingMs { get; set; }
    public int? Heat { get; set; }
    public int? Lane { get; set; }
}

public class ResultUpdateDto
{
    public int TimingMs { get; set; }
    public int? Heat { get; set; }
    public int? Lane { get; set; }
}
