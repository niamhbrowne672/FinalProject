namespace FinalProject.Data.Entities;

public class EventLike
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string UserId { get; set; }
    public Event Event { get; set; }
}