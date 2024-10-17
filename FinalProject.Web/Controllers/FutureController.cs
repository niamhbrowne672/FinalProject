// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authorization;
// using System.Security.Claims;

// using FinalProject.Data.Entities;
// using FinalProject.Data.Services;
// using FinalProject.Data.Security;
// using FinalProject.Web.Models.User;

// namespace FinalProject.Web.Controllers;

// public class FutureController : BaseController
// {
//     private readonly IEventService _eventService;

//     public FutureController(IEventService eventService)
//     {
//         _eventService = eventService;
//     }

//     // public IActionResult Index(int page = 1, int size = 20, string order = "id", string direction = "asc")
//     // {
//     //     var pagedEvents = _eventService.GetEvents(page, size, order, direction);
//     //     return View(pagedEvents);
//     // }


//     // GET: List of Future Events
//     public IActionResult Index(int page = 1, int size = 20, string order = "id", string direction = "asc")
//     {
//         var futureEvents = _eventService.GetFutureEvents(page, size, order, direction);
//         return View(futureEvents);
//     }

//     // GET: Create New Future Event (Admin Only)
//     [Authorize(Roles = "admin")]
//     public IActionResult Create()
//     {
//         return View();
//     }

//     // POST: Create New Future Event (Admin Only)
//     [Authorize(Roles = "admin")]
//     [HttpPost]
//     public IActionResult Create(Event newEvent)
//     {
//         if (ModelState.IsValid)
//         {
//             _eventService.AddEvent(newEvent);
//             return RedirectToAction("Index");
//         }
//         return View(newEvent);
//     }

//     // GET: Edit Event (Admin Only)
//     [Authorize(Roles = "admin")]
//     public IActionResult Edit(int id)
//     {
//         var eventToEdit = _eventService.GetEventById(id);
//         if (eventToEdit == null)
//         {
//             return NotFound();
//         }
//         return View(eventToEdit);
//     }

//     // POST: Edit Event (Admin Only)
//     [Authorize(Roles = "admin")]
//     [HttpPost]
//     public IActionResult Edit(Event updatedEvent)
//     {
//         if (ModelState.IsValid)
//         {
//             _eventService.UpdateEvent(updatedEvent);
//             return RedirectToAction("Index");
//         }
//         return View(updatedEvent);
//     }

//     // DELETE: Delete Event (Admin Only)
//     [Authorize(Roles = "admin")]
//     [HttpPost]
//     public IActionResult Delete(int id)
//     {
//         var eventToDelete = _eventService.GetEventById(id);
//         if (eventToDelete != null)
//         {
//             _eventService.DeleteEvent(id);
//         }
//         return RedirectToAction("Index");
//     }

//     // Method to move expired Future Events to Past Events
//     [HttpPost]
//     public void MovePastEvents()
//     {
//         var futureEvents = _eventService.GetFutureEvents(1, int.MaxValue, "id", "asc");
//         var expiredEvents = futureEvents.Data.Where(e => e.IsPast).ToList();

//         foreach (var evt in expiredEvents)
//         {
//             _eventService.MoveEventToPast(evt);  // Move without setting IsPast manually
//         }
//     }


// }

