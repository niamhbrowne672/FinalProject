using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace FinalProject.Data.Entities;

public class PastEventImage
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    [StringLength(200)]
    public string Description { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    public string UploadedBy { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public int UserId { get; set; }
    public User User{ get; set; }
}