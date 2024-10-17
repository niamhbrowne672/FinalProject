using FinalProject.Data.Entities;

namespace FinalProject.Data.Services
{
    public interface IEventService
    {
        // Get all events
        Paged<Event> GetEvents();
        Paged<Event> GetEvents(int page, int size, string orderBy = "id", string direction = "asc");

        // Get past events
        //Paged<Event> GetPastEvents(int page, int pageSize, string orderBy = "id", string direction = "asc");

        // Get future events
        //Paged<Event> GetFutureEvents(int page, int pageSize, string orderBy = "id", string direction = "asc");

        // Add a new event
        Event AddEvent(Event eventEntity);

        // Get an event by ID 
        //Event GetEventById(int id);

        // Update an event 
        //Event UpdateEvent(Event eventEntity);

        // Delete an event 
        //void DeleteEvent(int id);

        // Move an event to past
        //void MoveEventToPast(Event eventEntity); // New method for moving events to past
    }
}
