using System.ComponentModel.DataAnnotations;

namespace FinalProject.Data.Entities;

public class Review
{
    public int Id { get; set; }
    public DateTime On { get; set; } = DateTime.Now;

    [Required]
    public string Name { get; set; }

    [Required]
    [StringLength(500)]
    public string Comment { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set;}

    //review owned by an event
    public int EventId { get; set; }
    public Event Event { get; set; }
}