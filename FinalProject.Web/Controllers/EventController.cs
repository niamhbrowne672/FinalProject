using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Data.Entities;
using FinalProject.Data.Services;
using FinalProject.Data.Extensions;



namespace FinalProject.Web.Controllers;

public class EventController : BaseController
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    public IActionResult Index(string searchQuery, int page = 1, int size = 20, string order = "id", string direction = "asc")
    {
        var query = string.IsNullOrWhiteSpace(searchQuery) 
            ? _eventService.GetAllEvents()
            : _eventService.SearchEvents(searchQuery);

        //apply pagination, sorting etc
        var pagedEvents = query.ToPaged(page, size, order);

        //pass the search query back to the view for display in the search bar
        ViewBag.SearchQuery = searchQuery;

        return View(pagedEvents);
        
        // var query = _eventService.GetAllEvents();
        // var pagedEvents = query.ToPaged(page, size, order);
        // return View(pagedEvents);
    }

    public IActionResult Details(int id)
    {
        var eventItem = _eventService.GetEventById(id);

        if (eventItem == null)
        {
            Alert($"Event {id} not found.", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }
        return View(eventItem);
    }

    [Authorize(Roles = "admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Event e)
    {
        //validate title as unique
        if (_eventService.GetEventByTitle(e.Title) != null)
        {
            ModelState.AddModelError(nameof(e.Title), "Event is already in the database.");
        }

        //complete POST action to add event
        if (ModelState.IsValid)
        {
            e = _eventService.AddEvent(e);
            if (e != null)
            {
                return RedirectToAction(nameof(Details), new { id = e.Id });
            }
        }
        return View(e);
    }

    [Authorize(Roles = "admin")]
    public IActionResult Edit(int id)
    {
        //load the event using the service
        var eventItem = _eventService.GetEventById(id);

        //check ifevent is null
        if (eventItem == null)
        {
            Alert($"Event {id} not found.", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        return View(eventItem);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public IActionResult Edit(int id, Event updatedEvent)
    {
        //check if the provided even ID matches the ID of the updated post
        if (id != updatedEvent.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var existingEvent = _eventService.GetEventById(id);

            //check if the event exists in the database
            if (existingEvent == null)
            {
                Alert($"Event {id} not found.", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            //update the existing event entity with the new values
            existingEvent.Title = updatedEvent.Title;
            existingEvent.EventTime = updatedEvent.EventTime;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.ImageUrl = updatedEvent.ImageUrl;

            //save the changes to the database
            var savedEvent = _eventService.UpdateEvent(existingEvent);

            if (savedEvent != null)
            {
                Alert($"Event updated.", AlertType.success);
                return RedirectToAction(nameof(Details), new { id = savedEvent.Id });
            }
        }
        //if ModelState is not valid or event update failed, redisplay the form for editing
        return View(updatedEvent);
    }

    [Authorize(Roles = "admin")]
    public IActionResult Delete(int id)
    {
        //load the event using the service
        var eventItem = _eventService.GetEventById(id);
        //check the returned event is not null
        if (eventItem == null)
        {
            Alert($"Event {id} could not be deleted.", AlertType.danger);
            return RedirectToAction(nameof(Index));
        }

        //pass event to view for deletion confirmation
        return View(eventItem);
    }

    [HttpPost, ActionName("DeleteConfirm")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteConfirmed(int id)
    {
        //delete event via service
        var deleted = _eventService.DeleteEvent(id);
        if (deleted)
        {
            Alert("Event Deleted.", AlertType.success);
        }
        else
        {
            Alert("Event could not be deleted", AlertType.warning);
        }

        //redirect to the index view
        return RedirectToAction(nameof(Index));
    }

    //========================= Event Review Management ====================
    public IActionResult ReviewCreate(int id)
    {
        var eventItem = _eventService.GetEventById(id);
        if (eventItem == null)
        {
            Alert("Event does not exist.", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        //create a review view model and set foreign key
        var review = new Review { EventId = id };
        //render blank form
        return View(review);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ReviewCreate(Review review)
    {
        if (ModelState.IsValid)
        {                
            var createdReview = _eventService.CreateReview(review); 
            if (createdReview != null)
            {
                Alert("Review Created Successfully.", AlertType.success);
                return RedirectToAction(nameof(Details), new { id = review.EventId});
            }
            else
            {
                Alert("Review could not be created.", AlertType.warning);
            }
        }
        // redisplay the form for editing
        return View(review);
    }

    [Authorize(Roles = "admin")]
    public IActionResult ReviewDelete(int id)
    {
        // load the ticket using the service
        var review = _eventService.GetReview(id);
        // check the returned Review is not null
        if (review == null)
        {
            Alert("Review does not exist.", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }     
        
        // pass review to view for deletion confirmation
        return View(review);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "admin")]
    public IActionResult ReviewDeleteConfirm(int id, int eventId)
    {
        var deleted = _eventService.DeleteReview(id);

        if (deleted)
            {
                Alert("Review deleted Successfully.", AlertType.success);
            }
            else
            {
                Alert("Review could not be deleted.", AlertType.warning);
            }

        // redirect to the event details view
        return RedirectToAction(nameof(Details), new { Id = eventId });
    }


   // Past events page
    public IActionResult Past(int page = 1, int size = 10, string order = "eventTime", string direction = "desc")
    {
        var pastEvents = _eventService.GetPastEvents(); // Fetch all past events

        // Apply pagination and sorting
        var pagedPastEvents = pastEvents.ToPaged(page, size, order, direction);

        return View(pagedPastEvents);
    }

    // Display images for a specific past event
    public IActionResult PastEventImages(int eventId, int page = 1, int size = 20)
    {
        var images = _eventService.GetPastEventImages(eventId);

        if (images == null || !images.Any())
        {
            Alert($"No images found for Event {eventId}.", AlertType.warning);
            return RedirectToAction(nameof(Past));
        }

        // Apply pagination
        var pagedImages = images.AsQueryable().ToPaged(page, size);

        return View(pagedImages);
    }

    public IEnumerable<Event> GetPastEvents()
    {
        return _eventService.GetPastEvents().Where(e => e.EventTime < DateTime.Now).ToList();
    }
}
