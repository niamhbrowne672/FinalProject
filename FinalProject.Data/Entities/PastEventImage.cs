using System;
using System.ComponentModel.DataAnnotations;
namespace FinalProject.Data.Entities;

public class PastEventImage
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ImageTitle { get; set; }

    [Required]
    [MaxLength(200)]
    public string ImageDescription { get; set; }

    [Required]
    public string GalleryImageUrl { get; set; } 

    public string ImagePostedBy { get; set; } 
}