using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FinalProject.Web.Models;
using FinalProject.Data.Services;

namespace FinalProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEventService _eventService;

        public HomeController(ILogger<HomeController> logger, IEventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
        }

        public IActionResult Index()
        {
            var upcomingEvents = _eventService.GetUpComingEvents(3);
            return View(upcomingEvents);
        }

        [Authorize]
        public IActionResult Secure()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }

        public IActionResult Training()
        {
            return View();
        }

        public IActionResult Recipes()
        {
            return View();
        }

         public IActionResult Breed()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
