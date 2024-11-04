using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using FinalProject.Data.Entities;
using FinalProject.Data.Services;
using FinalProject.Data.Security;
using FinalProject.Web.Models.User;

namespace FinalProject.Web.Controllers;

public class EventController : BaseController
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    public ActionResult Index(int page = 1, int size = 20, string order = "id", string direction = "asc")
    {
        var paged = _eventService.GetEvents(page, size, order, direction);
        return View(paged);
    }


    // // HTTP GET - Display paged list of future events
    // public ActionResult Future(int page = 1, int size = 20, string order = "id", string direction = "asc")
    // {
    //     var futureEvents = _eventService.GetFutureEvents(page, size, order, direction);
    //     return View(futureEvents);
    // }

    // // HTTP GET - Display paged list of past events
    // public ActionResult Past(int page = 1, int size = 20, string order = "id", string direction = "asc")
    // {
    //     var pastEvents = _eventService.GetPastEvents(page, size, order, direction);
    //     return View(pastEvents);
    // }

    // // HTTP GET - Display calendar with future events
    // public ActionResult Calendar(int page = 1, int pageSize = 100)
    // {
    //     var events = _eventService.GetFutureEvents(page, pageSize);  // No pagination for calendar view
    //     return View(events);
    // }

    // HTTP GET - Show the Create event form
    public IActionResult Create()
    {
        return View();
    }

    // HTTP POST - Handle the submission of the Create event form
    [HttpPost]
    [ValidateAntiForgeryToken] // Prevent CSRF attacks
    public IActionResult Create(Event eventEntity)
    {
        if (ModelState.IsValid)
        {
            _eventService.AddEvent(eventEntity);
            return RedirectToAction("Future"); // Redirect to future events after creation
        }
        return View(eventEntity); // Return the view with the event data if validation fails
    }

    // // HTTP GET - Show the Edit event form
    // public IActionResult Edit(int id)
    // {
    //     var eventEntity = _eventService.GetEventById(id);
    //     if (eventEntity == null)
    //     {
    //         return NotFound(); // Return 404 if event not found
    //     }
    //     return View(eventEntity);
    // }

    // // HTTP POST - Handle the submission of the Edit event form
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public IActionResult Edit(Event eventEntity)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         _eventService.UpdateEvent(eventEntity);
    //         return RedirectToAction("Future"); // Redirect to future events after editing
    //     }
    //     return View(eventEntity); // Return the view with the event data if validation fails
    // }

    // // HTTP GET - Show the Delete confirmation page
    // public IActionResult Delete(int id)
    // {
    //     var eventEntity = _eventService.GetEventById(id);
    //     if (eventEntity == null)
    //     {
    //         return NotFound(); // Return 404 if event not found
    //     }
    //     return View(eventEntity);
    // }

    // // HTTP POST - Handle the deletion of an event
    // [HttpPost, ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public IActionResult DeleteConfirmed(int id)
    // {
    //     _eventService.DeleteEvent(id);
    //     return RedirectToAction("Future"); // Redirect to future events after deletion
    // }
}
