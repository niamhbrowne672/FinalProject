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

// public class PastController : BaseController
// {
//     private readonly IEventService _eventService;

//     public PastController(IEventService eventService)
//     {
//         _eventService = eventService;
//     }

//     // HTTP GET - Display Paged List of Past Events
//     public ActionResult Index(int page = 1, int size = 20, string order = "date", string direction = "asc")
//     {
//         var paged = _eventService.GetPastEvents(page, size, order, direction);
//         return View(paged);
//     }

// //    // HTTP POST - Upload Image for an Event
// //     [HttpPost]
// //     [Authorize]
// //     public async Task<IActionResult> UploadImage(int eventId, IFormFile imageFile)
// //     {
// //         if (imageFile != null && imageFile.Length > 0)
// //         {
// //             try
// //             {
// //                 // Assuming you have a method to handle saving the image
// //                 await _eventService.UploadImage(eventId, imageFile);
// //                 // Optional: You can add a success message here
// //             }
// //             catch (Exception ex)
// //             {
// //                 // Handle exceptions (e.g., log the error, return an error view)
// //                 ModelState.AddModelError("", "Image upload failed: " + ex.Message);
// //             }
// //         }
// //         else
// //         {
// //             ModelState.AddModelError("", "No file selected for upload.");
// //         }

// //         // Redirect back to the index after upload attempt
// //         return RedirectToAction("Index");
// //     }

// }

