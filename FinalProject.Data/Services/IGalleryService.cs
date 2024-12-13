using FinalProject.Data.Entities;

namespace FinalProject.Data.Services;

public interface IGalleryService
{
    IEnumerable<PastEventImage> GetAllImages();
    PastEventImage AddImage(PastEventImage image);
    bool DeleteImage(int id);
}