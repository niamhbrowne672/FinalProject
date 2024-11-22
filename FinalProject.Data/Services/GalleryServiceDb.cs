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

    // Add a new image or set of images to the gallery
    public PastEventImage AddImage(PastEventImage image)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(image.ImageTitle) ||
            string.IsNullOrWhiteSpace(image.ImageDescription) ||
            image.GalleryImageUrls == null || !image.GalleryImageUrls.Any())
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

        // delete physical files if required
        foreach (var imageUrl in imageToDelete.GalleryImageUrls)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        return true;
    }
}