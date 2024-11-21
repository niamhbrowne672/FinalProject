using FinalProject.Data.Entities;
using FinalProject.Data.Repositories;

namespace FinalProject.Data.Services;

public class GalleryServiceDb : IGalleryService
{
    private readonly DatabaseContext ctx;

    public GalleryServiceDb(DatabaseContext ctx)
    {
        this.ctx = ctx;
    }

    // Retrieve all images from the gallery
    public IEnumerable<PastEventImage> GetAllImages()
    {
        return ctx.PastEventImages.ToList();
    }

    // Add a new image to the gallery
    public PastEventImage AddImage(PastEventImage image)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(image.ImageTitle) ||
            string.IsNullOrWhiteSpace(image.ImageDescription) ||
            string.IsNullOrWhiteSpace(image.GalleryImageUrl))
        {
            return null;
        }

        // Save the image to the database
        ctx.PastEventImages.Add(image);
        ctx.SaveChanges();
        return image;
    }

    // Delete an image by its ID
    public bool DeleteImage(int id)
    {
        // Find the image in the database
        var imageToDelete = ctx.PastEventImages.FirstOrDefault(i => i.Id == id);
        if (imageToDelete == null)
        {
            return false; // Return false if the image doesn't exist
        }

        // Remove the image and save changes
        ctx.PastEventImages.Remove(imageToDelete);
        ctx.SaveChanges();
        return true;
    }
}