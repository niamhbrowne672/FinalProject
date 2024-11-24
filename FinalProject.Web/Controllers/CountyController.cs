using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FinalProject.Data.Services;
using FinalProject.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Web.Controllers;

namespace FinalProject.Data.Controllers
{
  
    [Authorize]
    public class CountyController : BaseController
    {
        private ICalendarService _svc;

        public CountyController(ICalendarService svc)
        {
            _svc = svc;
        }
   
        public IActionResult Index()
        {
            var counties = _svc.GetCounties();
            return View(counties);
        }

    }
}
