using FinalProject.Data.Entities;
using FinalProject.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data.Extensions;

namespace FinalProject.Data.Services;

public class EventServiceDb : IEventService
{
    private readonly DatabaseContext ctx;

    public EventServiceDb(DatabaseContext ctx)
    {
        this.ctx = ctx;
    }

    public void Initialise()
    {
        ctx.Initialise();
    }

    public Paged<Event> GetEvents(int page, int pageSize, string orderBy = "id", string direction = "asc")
    {
        var query = (orderBy.ToLower(), direction.ToLower()) switch
        {
            ("id", "asc") => ctx.Events.OrderBy(r => r.Id),
            ("id", "desc") => ctx.Events.OrderByDescending(r => r.Id),
            ("title", "asc") => ctx.Events.OrderBy(r => r.Title),
            ("title", "desc") => ctx.Events.OrderByDescending(r => r.Title),
            ("eventTime", "asc") => ctx.Events.OrderBy(r => r.EventTime),
            ("eventTime", "desc") => ctx.Events.OrderByDescending(r => r.EventTime),
            // ("decription","asc")      => ctx.Events.OrderBy(r => r.Description),
            // ("description","desc")     => ctx.Events.OrderByDescending(r => r.Description),
            ("location", "asc") => ctx.Events.OrderBy(r => r.Location),
            ("location", "desc") => ctx.Events.OrderByDescending(r => r.Location),
            _ => ctx.Events.OrderBy(r => r.Id)
        };

        return query.ToPaged(page, pageSize, orderBy, direction);
    }

    public IQueryable<Event> SearchEvents(string searchQuery)
    {
        return ctx.Events.Where(e => e.Title.Contains(searchQuery) || e.Location.Contains(searchQuery));
    }

    public IQueryable<Event> GetAllEvents()
    {
        return ctx.Events;
    }

    public Paged<Event> GetEvents()
    {
        return ctx.Events.ToPaged();
    }

    //get the event by the id
    public Event GetEventById(int id)
    {
        return ctx.Events.Include(e => e.Reviews).FirstOrDefault(e => e.Id == id);
    }

    public Event GetEventByTitle(string title)
    {
        return ctx.Events
        .Include(e => e.Reviews)
        .FirstOrDefault(e => e.Title == title);
    }

    //add a new event
    public Event AddEvent(Event e)
    {
        //check if event exists
        if (GetEventByTitle(e.Title) != null)
        {
            return null;
        }

        //create new event
        var newEvent = new Event
        {
            Id = e.Id,
            Title = e.Title,
            EventTime = e.EventTime,
            Location = e.Location,
            Description = e.Description,
            ImageUrl = e.ImageUrl
        };
        ctx.Events.Add(newEvent);
        ctx.SaveChanges();
        return newEvent;
    }

    //delete the event identified by Id returning true if deleted and false if not found
    public bool DeleteEvent(int id)
    {
        var eventToDelete = GetEventById(id);
        if (eventToDelete == null)
        {
            return false;
        }
        ctx.Events.Remove(eventToDelete);
        ctx.SaveChanges();
        return true;
    }

    //Update the event with the details in updated
    public Event UpdateEvent(Event updated)
    {
        //verify the event exists
        var existingEvent = GetEventById(updated.Id);
        if (existingEvent == null)
        {
            return null;
        }

        //verify event is still unique
        var eventWithSameTitle = GetEventByTitle(updated.Title);
        if (eventWithSameTitle != null && eventWithSameTitle.Id != updated.Id)
        {
            Console.WriteLine($"Event with title '{updated.Title}' already exists with a different ID.");
            return null;
        }

        //update the details of the event retrieved and save
        existingEvent.Title = updated.Title;
        existingEvent.EventTime = updated.EventTime;
        existingEvent.Location = updated.Location;
        existingEvent.Description = updated.Description;
        existingEvent.ImageUrl = updated.ImageUrl;

        ctx.SaveChanges();
        Console.WriteLine($"Event with ID {updated.Id} updated successfully.");
        return existingEvent;
    }

    // ================= Review Management =================
    public Review CreateReview(int eventId, string name, string comment, int rating)
    {
        var eventToReview = GetEventById(eventId);
        if (eventToReview == null)
        {
            return null;
        }

        var review = new Review
        {
            Name = name,
            Comment = comment,
            Rating = rating,
            On = DateTime.Now
        };

        eventToReview.Reviews.Add(review);
        ctx.SaveChanges();
        return review;
    }

    //convenience method to align Review creation format used in Event Creation
    public Review CreateReview(Review review)
    {
        return CreateReview(review.EventId, review.Name, review.Comment, review.Rating);
    }

    public Review GetReview(int id)
    {
        return ctx.Reviews.Include(r => r.Event).FirstOrDefault(r => r.Id == id);
    }

    public bool DeleteReview(int id)
    {
        //find review
        var reviewToDelete = GetReview(id);
        if (reviewToDelete == null)
        {
            return false;
        }

        //remove review
        ctx.Reviews.Remove(reviewToDelete);

        ctx.SaveChanges();
        return true;
    }

    //retrieve all reviews and the events associated with the review
    public IList<Review> GetAllReviews()
    {
        return ctx.Reviews.Include(r => r.Event).ToList();
    }

    public Review UpdateReview(int id, Review updated)
    {
        var existingReview = GetReview(id);

        if (existingReview != null) return null;

        existingReview.Name = updated.Name;
        existingReview.On = updated.On;
        existingReview.Comment = updated.Comment;
        existingReview.Rating = updated.Rating;

        ctx.SaveChanges();
        return existingReview;
    }
}
