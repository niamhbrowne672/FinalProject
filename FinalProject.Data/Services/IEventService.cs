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

        // Get an event by ID 
        Event GetEventById(int id);

        //get all events
        IQueryable<Event> GetAllEvents();
        IQueryable<Event> GetAllEvents(string userId);

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

        //likes
        bool LikeEvent(int eventId);
        ToggleLikeResult ToggleLike(int eventId, string userId);

        //=================== Home Page Upcomming Events cards ========================
        IList<Event> GetUpComingEvents(int count);

        //=================== Register to event ====================
        bool IsUserRegistered(int eventId, string userId);
        bool RegisterUserForEvent(int eventId, string userId);
    }
}