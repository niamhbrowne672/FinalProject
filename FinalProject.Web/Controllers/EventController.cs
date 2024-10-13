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

    // HTTP GET - Display paged list of future events
    public ActionResult Future(int page = 1, int size = 20, string order = "id", string direction = "asc")
    {
        var futureEvents = _eventService.GetFutureEvents(page, size, order, direction);
        return View(futureEvents);
    }

    // HTTP GET - Display paged list of past events
    public ActionResult Past(int page = 1, int size = 20, string order = "id", string direction = "asc")
    {
        var pastEvents = _eventService.GetPastEvents(page, size, order, direction);
        return View(pastEvents);
    }

    // HTTP GET - Display calendar with future events
    public ActionResult Calendar(int page = 1, int pageSize = 100)
    {
        var events = _eventService.GetFutureEvents(page, pageSize);  // No pagination for calendar view
        return View(events);
    }
}
