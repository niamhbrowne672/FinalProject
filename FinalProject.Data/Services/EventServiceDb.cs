using FinalProject.Data.Entities;
using FinalProject.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data.Extensions;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Collections.Immutable;

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
        var lowerQuery = searchQuery.ToLower();
        return ctx.Events.Where(e => e.Title.ToLower().Contains(searchQuery) || e.Location.ToLower().Contains(searchQuery));
    }

    public IQueryable<Event> GetAllEvents()
    {
        return ctx.Events.Include(e => e.Reviews);
    }

    public IQueryable<Event> GetAllEvents(string userId)
    {
        var eventsQuery = ctx.Events.Include(e => e.Reviews);
        if (!string.IsNullOrEmpty(userId))
        {
            var eventsWithUserLikes = eventsQuery.Select(e => new Event
            {
                Id = e.Id,
                Title = e.Title,
                EventTime = e.EventTime,
                Location = e.Location,
                Description = e.Description,
                ImageUrl = e.ImageUrl,
                Likes = e.Likes,
                Reviews = e.Reviews,
                UserHasLiked = ctx.EventLikes.Any(like => like.EventId == e.Id && like.UserId == userId)
            });
            return eventsWithUserLikes;
        }
        return eventsQuery;
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
    // public Review CreateReview(Review review)
    // {
    //     return CreateReview(review.EventId, review.Name, review.Comment, review.Rating);
    // }
    public Review CreateReview(Review review)
    {
        var eventToReview = GetEventById(review.EventId);
        if (eventToReview == null)
        {
            return null;
        }

        review.On = DateTime.Now; // Ensure timestamp is set
        ctx.Reviews.Add(review); // Add review to context
        ctx.SaveChanges();       // Save changes to database
        return review;
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

    //likes to show how many people are going to an event
    public bool LikeEvent(int eventId)
    {
        var eventToLike = GetEventById(eventId);
        if (eventToLike == null) return false;

        eventToLike.Likes++;
        ctx.SaveChanges();
        return true;
    }

    public ToggleLikeResult ToggleLike(int eventId, string userId)
    {
        var eventToLike = GetEventById(eventId);
        if (eventToLike == null)
        {
            return new ToggleLikeResult { Success = false, Message = "Event not found." };
        }

        //check if user has already liked the event
        var userLike = ctx.EventLikes.FirstOrDefault(like => like.EventId == eventId && like.UserId == userId);

        if (userLike != null)
        {
            //unlike the event
            ctx.EventLikes.Remove(userLike);
            eventToLike.Likes--;
        }
        else
        {
            //like the event
            ctx.EventLikes.Add(new EventLike { EventId = eventId, UserId = userId });
            eventToLike.Likes++;
        }

        ctx.SaveChanges();

        return new ToggleLikeResult
        {
            Success = true,
            Liked = userLike == null,
            Likes = eventToLike.Likes
        };
    }

    public IList<Event> GetUpComingEvents(int count)
    {
        return ctx.Events
            .Where(e => e.EventTime > DateTime.Now)
            .OrderBy(e => e.EventTime)
            .Take(count)
            .ToList();
    }
}
