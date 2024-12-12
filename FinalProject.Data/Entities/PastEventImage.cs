using System.ComponentModel.DataAnnotations;
namespace FinalProject.Data.Entities;

public class PastEventImage
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string ImageTitle { get; set; }

    [Required]
    [MaxLength(90)]
    public string ImageDescription { get; set; }
    [Required]
    public List<string> GalleryImageUrls { get; set; } = new List<string>();

    [Required]
    public string ImagePostedBy { get; set; } 
}