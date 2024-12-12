namespace FinalProject.Data.Entities;
public class Calendar
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Location { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int CountyId { get; set; }
    public int UserId { get; set; }

}
