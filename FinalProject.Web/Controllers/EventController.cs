using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FinalProject.Data.Entities;
using FinalProject.Data.Services;
using FinalProject.Data.Extensions;



namespace FinalProject.Web.Controllers;

public class EventController : BaseController
{
    private readonly IEventService _eventService;
    private readonly IUserService _userService;
    private readonly IMailService _mailer;

    public EventController(IEventService eventService, IUserService userService, IMailService mailer)
    {
        _eventService = eventService;
        _userService = userService;
        _mailer = mailer;
    }

    public IActionResult Index(string searchQuery, int page = 1, int size = 20, string order = "id", string direction = "asc")
    {
        var userId = User.Identity.Name;

        var query = string.IsNullOrWhiteSpace(searchQuery)
            ? _eventService.GetAllEvents(userId)
            : _eventService.SearchEvents(searchQuery);

        //apply pagination, sorting etc
        var pagedEvents = query.ToPaged(page, size, order);

        //pass the search query back to the view for display in the search bar
        ViewBag.SearchQuery = searchQuery;

        if (!pagedEvents.Data.Any() && !string.IsNullOrWhiteSpace(searchQuery))
        {
            Alert($"No events found matching '{searchQuery}'. Please check the spelling or try again.", AlertType.warning);
        }

        return View(pagedEvents);
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

    [Authorize(Roles = "admin,manager")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Event e, IFormFile EventImage)
    {

        if (e.EndTime <= e.EventTime)
        {
            ModelState.AddModelError("EndTime", "End Time must be after the Start Time.");
        }

        //validate title as unique
        if (_eventService.GetEventByTitle(e.Title) != null)
        {
            ModelState.AddModelError(nameof(e.Title), "Event is already in the database.");
        }

        //handle image upload
        if (EventImage != null && EventImage.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(EventImage.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                EventImage.CopyTo(fileStream);
            }
            e.ImageUrl = $"/uploads/{uniqueFileName}";

            Console.WriteLine($"ImageUrl assigned: {e.ImageUrl}");
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

    [Authorize(Roles = "admin,manager")]
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
    [Authorize(Roles = "admin,manager")]
    public IActionResult Edit(int id, Event updatedEvent, IFormFile uploadedImage)
    {
        if (updatedEvent.EndTime <= updatedEvent.EventTime)
        {
            ModelState.AddModelError("EndTime", "End Time must be after the Start Time.");
        }

        if (id != updatedEvent.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var existingEvent = _eventService.GetEventById(id);

            if (existingEvent == null)
            {
                Alert($"Event {id} not found.", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }

            // Update other properties
            existingEvent.Title = updatedEvent.Title;
            existingEvent.EventTime = updatedEvent.EventTime;
            existingEvent.EndTime = updatedEvent.EndTime;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.Description = updatedEvent.Description;

            // Handle the uploaded image
            if (uploadedImage != null && uploadedImage.Length > 0)
            {
                // Define a folder path for image uploads
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                // Ensure the uploads folder exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate a unique file name
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save the image to the server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    uploadedImage.CopyTo(fileStream);
                }

                // Update the image URL
                existingEvent.ImageUrl = $"/uploads/{uniqueFileName}";
            }

            // Save changes
            var savedEvent = _eventService.UpdateEvent(existingEvent);

            if (savedEvent != null)
            {
                Alert("Event updated successfully.", AlertType.success);
                return RedirectToAction(nameof(Details), new { id = savedEvent.Id });
            }
        }

        return View(updatedEvent);
    }


    [Authorize(Roles = "admin,manager")]
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
    [Authorize(Roles = "admin,manager")]
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
        return PartialView("_CreateReviewModal", review);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ReviewCreate(Review review)
    {
        var eventItem = _eventService.GetEventById(review.EventId);
        if (eventItem == null || eventItem.EventTime > DateTime.Now)
        {
            Alert("Reviews can only be added after the event date.", AlertType.warning);
            return RedirectToAction(nameof(Details), new { id = review.EventId });
        }
        if (ModelState.IsValid)
        {
            var createdReview = _eventService.CreateReview(review);
            if (createdReview != null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                return RedirectToAction(nameof(Details), new { id = review.EventId });
            }
            return Json(new { success = false, message = "Failed to create review." });
        }
        return Json(new { success = false, message = "Invalid input." });
    }




    [Authorize(Roles = "admin,manager")]
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
    [Authorize(Roles = "admin,manager")]
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

    //like button
    [HttpPost]
    [Authorize]
    public IActionResult ToggleLike(int id)
    {
        var userId = User.Identity.Name;
        var result = _eventService.ToggleLike(id, userId);

        if (!result.Success)
        {
            return BadRequest(new { success = false, message = result.Message });
        }

        return Ok(new
        {
            success = true,
            liked = result.Liked,
            likes = result.Likes
        });
    }

    public IActionResult GetUpComingEvents()
    {
        var events = _eventService.GetUpComingEvents(3);
        return PartialView("_UpcomingEvents", events);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Register(int eventId)
    {
        var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        if (string.IsNullOrEmpty(userEmail))
        {
            Alert("Unable to identify the user. Please log in again.", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        var user = _userService.GetUserByEmail(userEmail);
        if (user == null)
        {
            Alert("User not found.", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        var eventItem = _eventService.GetEventById(eventId);
        if (eventItem == null)
        {
            Alert("Event not found.", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        if (_eventService.IsUserRegistered(eventId, user.Id.ToString()))
        {
            Alert("You are already registered for this event.", AlertType.info);
            return RedirectToAction(nameof(Details), new { id = eventId });
        }

        var success = _eventService.RegisterUserForEvent(eventId, user.Id.ToString());

        if (success)
        {
            Alert($"You've registered for {eventItem.Title}! We can't wait to see you there.", AlertType.success);

            var subject = $"You're Registered for {eventItem.Title}";
            var body = $@"
            <h3>Thank you for registering!</h3>
            <p>You have successfully registered for the event: <strong>{eventItem.Title}</strong>.</p>
            <p>Date & Time: {eventItem.EventTime:MMMM dd, yyyy - HH:mm}</p>
            <p>Location: {eventItem.Location}</p>
            <p>We can't wait to see you there!</p>";

            var mailSuccess = await _mailer.SendMailAsync(subject, body, userEmail);

            if (!mailSuccess)
            {
                Alert("Your registration is confirmed, but we couldn't send you an email. Please contact us for details.", AlertType.warning);
            }

            return RedirectToAction(nameof(Details), new { id = eventId });
        }
        else
        {
            Alert("There was an issue registering for the event. Please try again.", AlertType.danger);
            return RedirectToAction(nameof(Details), new { id = eventId });
        }
    }
}