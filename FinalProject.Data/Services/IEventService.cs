using FinalProject.Data.Entities;
using FinalProject.Data.Extensions;

namespace FinalProject.Data.Services
{
    public interface IEventService
    {
        // Get all events
        Paged<Event> GetEvents();
        Paged<Event> GetEvents(int page, int size, string orderBy = "id", string direction = "asc");

        // Add a new event
        Event AddEvent(Event e);

        Event GetEventByTitle(string title);

        //Event GetAllEvents();

        // Get an event by ID 
        Event GetEventById(int id);

        //get all events
        IQueryable<Event> GetAllEvents();

        // Update an event 
        Event UpdateEvent(Event updated);

        // Delete an event 
        bool DeleteEvent(int id);

        //search bar
        IQueryable<Event> SearchEvents(string searchQuery);

        //============== Review Management =============
        Review CreateReview(int id, string name, string comment, int rating);
        Review CreateReview(Review review);
        Review GetReview(int id);
        bool DeleteReview(int id);
        IList<Review> GetAllReviews();

        //Past Event Gallery
        IList<PastEventImage> GetPastEventImages(int eventId);
        PastEventImage AddPastEventImage(int eventId, PastEventImage newImage);
        bool DeletePastEventImage(int imageId);
        IQueryable<Event> GetPastEvents();
        //IEnumerable<Event> GetAllEvnets();
    }
}
