using System;
using System.ComponentModel.DataAnnotations;
namespace FinalProject.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Bogus.Extensions.UnitedKingdom;

public class Event
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public DateTime EventTime { get; set; }
    public DateTime EndTime { get; set; }
    [Required]
    public string Location { get; set; }  // I can add Google Maps URL here later
    public string Description { get; set; }
    public string ImageUrl { get; set; } 
    public int? UserId { get; set; }  // Foreign key for user who created the event
    public User User { get; set; }
    public string Breed { get; set; }

    // public bool IsPast => Date < DateTime.Now;

    public IList<Review> Reviews { get; set; } = new List<Review>();
    public string Query { get; set; } = string.Empty;
    public int Rating { get; set; }

    //past event gallery
    public IList<PastEventImage> PastEventImages { get; set; } = new List<PastEventImage>();

    //like button
    public int Likes { get; set; }
    [NotMapped]
    public bool UserHasLiked { get; set; }

    //Registration
    public ICollection<Registration> Registrations { get; set; } = new List<Registration>();

}


