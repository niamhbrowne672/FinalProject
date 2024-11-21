using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Data.Entities;
using FinalProject.Data.Services;
using FinalProject.Data.Extensions;

namespace FinalProject.Web.Controllers;

public class GalleryController : BaseController
{
    private readonly IGalleryService _galleryService;

    public GalleryController(IGalleryService galleryService)
    {
        _galleryService = galleryService;
    }

    // Display all past event images
    public IActionResult Past()
    {
        var images = _galleryService.GetAllImages();
        return View(images);
    }

    // Add a new image
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddImage(PastEventImage image, IFormFile ImageFile)
    {
        if (ImageFile != null && ImageFile.Length > 0)
        {
            // Save the uploaded image file
            var fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName) + "-" + Guid.NewGuid().ToString("N") + Path.GetExtension(ImageFile.FileName);
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/gallery");

            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            var filePath = Path.Combine(uploadsDir, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                ImageFile.CopyTo(stream);
            }

            // Set the image path
            image.GalleryImageUrl = $"/images/gallery/{fileName}";

            // Save the image details to the database
            var addedImage = _galleryService.AddImage(image);

            if (addedImage != null)
            {
                Alert("Image added successfully.", AlertType.success);
                return RedirectToAction(nameof(Past));
            }
        }

        // Handle errors
        Alert("Error adding image. Please check your inputs and try again.", AlertType.warning);
        return RedirectToAction(nameof(Past));
    }

    // Delete an image (Admin only)
    [HttpPost]
    [Authorize(Roles = "admin")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteImage(int id)
    {
        var success = _galleryService.DeleteImage(id);
        if (success)
        {
            Alert("Image deleted successfully.", AlertType.success);
        }
        else
        {
            Alert("Error deleting image. Please try again.", AlertType.warning);
        }

        return RedirectToAction(nameof(Past));
    }
}