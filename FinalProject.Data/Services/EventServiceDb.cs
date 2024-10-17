using FinalProject.Data.Entities;
using FinalProject.Data.Repositories;

using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data.Services;

public class EventServiceDb : IEventService
{
    private readonly DatabaseContext ctx;

    public EventServiceDb(DatabaseContext ctx)
    {
        this.ctx = ctx;
    }

    public Event AddEvent(Event eventEntity)
    {
        ctx.Events.Add(eventEntity);
        ctx.SaveChanges();
        return eventEntity;
    }

    public Paged<Event> GetEvents(int page, int pageSize, string orderBy = "id", string direction = "asc")
    {
        var query = (orderBy.ToLower(),direction.ToLower()) switch
        {
            ("id","asc")         => ctx.Events.OrderBy(r => r.Id),
            ("id","desc")        => ctx.Events.OrderByDescending(r => r.Id),
            ("title","asc")      => ctx.Events.OrderBy(r => r.Title),
            ("title","desc")     => ctx.Events.OrderByDescending(r => r.Title),
            ("eventTime","asc")      => ctx.Events.OrderBy(r => r.EventTime),
            ("eventTime","desc")     => ctx.Events.OrderByDescending(r => r.EventTime),
            // ("decription","asc")      => ctx.Events.OrderBy(r => r.Description),
            // ("description","desc")     => ctx.Events.OrderByDescending(r => r.Description),
            ("location","asc")  => ctx.Events.OrderBy(r => r.Location),
            ("location","desc") => ctx.Events.OrderByDescending(r => r.Location),
            _                    => ctx.Events.OrderBy(r => r.Id)
        };

        return query.ToPaged(page,pageSize,orderBy,direction);
    }

    public Paged<Event> GetEvents()
    {
        return ctx.Events.ToPaged();
    }

    public void Initialise()
    {
        ctx.Initialise();
    }
}

// using FinalProject.Data.Entities;
// using FinalProject.Data.Repositories;
// using Microsoft.EntityFrameworkCore;

// namespace FinalProject.Data.Services;

// public class EventServiceDb : IEventService
// {
//     private readonly DatabaseContext ctx;

//     public EventServiceDb(DatabaseContext ctx)
//     {
//         this.ctx = ctx;
//     }

//     // Add a new event
//     public Event AddEvent(Event eventEntity)
//     {
//         if (eventEntity.EventTime < DateTime.Now)
//         {
//             throw new ArgumentException("Cannot add an event with a past date."); // Ensure the date is valid
//         }

//         ctx.Events.Add(eventEntity);
//         ctx.SaveChanges();
//         return eventEntity;
//     }


//     // Get events with optional ordering
//     public List<Event> GetEvents(int page, int size, string orderBy = "id", string direction = "asc")
//     {
//         var query = (orderBy.ToLower(), direction.ToLower()) switch
//         {
//             ("id", "asc") => ctx.Events.OrderBy(r => r.Id),
//             ("id", "desc") => ctx.Events.OrderByDescending(r => r.Id),
//             ("title", "asc") => ctx.Events.OrderBy(r => r.Title),
//             ("title", "desc") => ctx.Events.OrderByDescending(r => r.Title),
//             ("date", "asc") => ctx.Events.OrderBy(r => r.EventTime),
//             ("date", "desc") => ctx.Events.OrderByDescending(r => r.EventTime),
//             _ => ctx.Events.OrderBy(r => r.Id)
//         };

//         return query.ToList(); // Return the list of events
//     }

//     // Get past events with pagination
//     public Paged<Event> GetPastEvents(int page, int pageSize, string orderBy = "id", string direction = "asc")
//     {
//         var query = (orderBy.ToLower(), direction.ToLower()) switch
//         {
//             ("id", "asc") => ctx.Events.Where(e => e.EventTime < DateTime.Now).OrderBy(r => r.Id),
//             ("id", "desc") => ctx.Events.Where(e => e.EventTime < DateTime.Now).OrderByDescending(r => r.Id),
//             ("title", "asc") => ctx.Events.Where(e => e.EventTime < DateTime.Now).OrderBy(r => r.Title),
//             ("title", "desc") => ctx.Events.Where(e => e.EventTime < DateTime.Now).OrderByDescending(r => r.Title),
//             ("date", "asc") => ctx.Events.Where(e => e.EventTime < DateTime.Now).OrderBy(r => r.EventTime),
//             ("date", "desc") => ctx.Events.Where(e => e.EventTime < DateTime.Now).OrderByDescending(r => r.EventTime),
//             _ => ctx.Events.Where(e => e.EventTime < DateTime.Now).OrderBy(r => r.Id)
//         };

//         // Return paginated results using ToPaged extension method
//         return query.ToPaged(page, pageSize, orderBy, direction);
//     }

//     // Get future events with pagination
//     public Paged<Event> GetFutureEvents(int page, int pageSize, string orderBy = "id", string direction = "asc")
//     {
//         var query = (orderBy.ToLower(), direction.ToLower()) switch
//         {
//             ("id", "asc") => ctx.Events.Where(e => e.EventTime >= DateTime.Now).OrderBy(r => r.Id),
//             ("id", "desc") => ctx.Events.Where(e => e.EventTime >= DateTime.Now).OrderByDescending(r => r.Id),
//             ("title", "asc") => ctx.Events.Where(e => e.EventTime >= DateTime.Now).OrderBy(r => r.Title),
//             ("title", "desc") => ctx.Events.Where(e => e.EventTime >= DateTime.Now).OrderByDescending(r => r.Title),
//             ("date", "asc") => ctx.Events.Where(e => e.EventTime >= DateTime.Now).OrderBy(r => r.EventTime),
//             ("date", "desc") => ctx.Events.Where(e => e.EventTime >= DateTime.Now).OrderByDescending(r => r.EventTime),
//             _ => ctx.Events.Where(e => e.EventTime >= DateTime.Now).OrderBy(r => r.Id)
//         };

//         return query.ToPaged(page, pageSize, orderBy, direction);
//     }

//     // Get an event by ID
//     public Event GetEventById(int id)
//     {
//         return ctx.Events.Find(id); // Retrieves the event by its ID
//     }

//     // Update an event
//     public Event UpdateEvent(Event eventEntity)
//     {
//         if (eventEntity.EventTime < DateTime.Now)
//         {
//             throw new ArgumentException("Cannot update an event to a past date."); // Ensure the date is valid
//         }

//         ctx.Events.Update(eventEntity);
//         ctx.SaveChanges();
//         return eventEntity;
//     }

//     // Delete an event
//     public void DeleteEvent(int id)
//     {
//         var eventEntity = ctx.Events.Find(id);
//         if (eventEntity != null)
//         {
//             ctx.Events.Remove(eventEntity);
//             ctx.SaveChanges();
//         }
//     }

//     // Move an event to past
//     public void MoveEventToPast(Event eventEntity)
//     {
//         if (eventEntity.EventTime >= DateTime.Now)
//         {
//             throw new ArgumentException("The event date must be in the past to move it to past events.");
//         }

//         // Logic to mark the event as past (or move it to a different collection)
//         ctx.Events.Update(eventEntity);
//         ctx.SaveChanges();
//     }

//     // Initialize database if needed
//     public void Initialise()
//     {
//         ctx.Initialise();
//     }
// }

