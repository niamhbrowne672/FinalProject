using System;
using System.ComponentModel.DataAnnotations;
namespace FinalProject.Data.Entities;

public class Event
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public DateTime EventTime { get; set; }
    // [Required]
    // public string Time { get; set; }  // time separate to date
    [Required]
    public string Location { get; set; }  // I can add Google Maps URL here later
    public string Description { get; set; }
    [Url]
    public string ImageUrl { get; set; }  // URL for the image
    public int UserId { get; set; }  // Foreign key for user who created the event

    // public bool IsPast => Date < DateTime.Now;
}

