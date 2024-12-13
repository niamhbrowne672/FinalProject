using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Data.Entities;
using FinalProject.Data.Services;

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddImage(PastEventImage image, IList<IFormFile> ImageFiles)
    {
        if (ImageFiles != null && ImageFiles.Count > 0)
        {
            var uploadedImageUrls = new List<string>();

            // Process each uploaded file
            foreach (var file in ImageFiles)
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "-" + Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/gallery");

                    if (!Directory.Exists(uploadsDir))
                    {
                        Directory.CreateDirectory(uploadsDir);
                    }

                    var filePath = Path.Combine(uploadsDir, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    uploadedImageUrls.Add($"/images/gallery/{fileName}");
                }
            }

            // Assign the uploaded image URLs to the entity
            image.GalleryImageUrls = uploadedImageUrls;

            // Save to the database
            var addedImage = _galleryService.AddImage(image);
            if (addedImage != null)
            {
                Alert("Image(s) added successfully.", AlertType.success);
                return RedirectToAction(nameof(Past));
            }
        }

        Alert("Error adding images. Please check your inputs and try again.", AlertType.warning);
        return RedirectToAction(nameof(Past));
    }



    // Delete an image (Admin only)
    [HttpPost]
    [Authorize(Roles = "admin,manager")]
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