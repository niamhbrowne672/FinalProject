using System;
using System.ComponentModel.DataAnnotations;
namespace FinalProject.Data.Entities;

public class Event
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public string Time { get; set; }  // If you want time separately
    [Required]
    public string Location { get; set; }  // You can add Google Maps URL here later
    public string Description { get; set; }
    public string ImageUrl { get; set; }  // URL for the image
    public int UserId { get; set; }  // Foreign key for user who created the event

    public bool IsPast => Date < DateTime.Now;
}

