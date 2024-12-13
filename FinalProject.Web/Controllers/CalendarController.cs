using Microsoft.AspNetCore.Mvc;
using FinalProject.Data.Services;
using FinalProject.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FinalProject.Web.Models.CalendarModels;

namespace FinalProject.Web.Controllers
{

    [Authorize]
    public class CalendarController : BaseController
    {
        private ICalendarService _svc;

        public CalendarController(ICalendarService svc)
        {
            _svc = svc;
        }

        public IActionResult County(int id, string starts = null)
        {
            // get logged in user
            var userId = User.GetSignedInUserId();
            var county = _svc.GetCounty(id);
            if (county == null)
            {
                Alert("County not found.", AlertType.warning);
                return RedirectToAction("Index", "County");
            }
            // if no start date default to today
            if (starts == null)
            {
                // uses startofweek extension method from Core project
                starts = DateTime.Now.StartOfWeek(DayOfWeek.Monday).ToString("yyyy-MM-dd");
            }

            // get user events for specific room
            var calendars = _svc.GetCalendars().Where(c => c.CountyId == id).ToList();

            var vm = new CalendarViewModel
            {
                CountyName = county.Name,
                UserId = userId,
                CountyId = id,
                Start = starts,
                End = starts,
                Calendars = calendars
            };
            return View(vm);

        }

        [Authorize(Roles = "admin")]
        public IActionResult Add(int id, DateTime start, DateTime end)
        {
            var userId = User.GetSignedInUserId();
            var county = _svc.GetCounty(id);
            if (county == null)
            {
                Alert("County not found.", AlertType.warning);
                return RedirectToAction("Index", "County");
            }

            var c = new Calendar
            {
                CountyId = id,
                UserId = userId,
                Start = start,
                End = end,
                Title = string.Empty,
                Location = string.Empty
            };

            var v = CalendarViewModel.FromCalendar(c);
            return View(v);
        }

        [HttpPost]
        public IActionResult Add(CalendarViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var added = _svc.AddCalendar(vm.ToCalendar());
                if (added != null)
                {
                    Alert($"Event '{vm.Title}' Successfully Created!", AlertType.info);
                    return RedirectToAction(nameof(County), new { id = added.CountyId });
                }

                Alert("Event could not be created", AlertType.warning);

            }
            return View(vm);
        }

        public IActionResult Edit(int id)
        {
            // extract users id and role from Identity
            var userId = User.GetSignedInUserId();

            // load the event
            var calendar = _svc.GetCalendar(id);
            if (calendar == null)
            {
                Alert("Event Does not exist", AlertType.warning);
                return RedirectToAction(nameof(Index), nameof(County));
            }
            // check user has privilege to edit event
            if (userId != calendar.UserId && !User.HasOneOfRoles(Role.admin.ToString()))
            {
                Alert("You do not have permission to edit this event.", AlertType.warning);
                return RedirectToAction(nameof(County), new { id = calendar.CountyId });
            }

            var vm = CalendarViewModel.FromCalendar(calendar);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind] CalendarViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var updated = _svc.UpdateCalendar(vm.ToCalendar());
                if (updated != null)
                {
                    Alert($"Event '{vm.Title}' Successfully Updated!", AlertType.info);
                    return RedirectToAction(nameof(County), new { Id = updated.CountyId });
                }
                else
                {
                    Alert("Event could not be updated.", AlertType.warning);
                }
            }
            return View(vm);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var calendar = _svc.GetCalendar(id);
            if (calendar == null)
            {
                Alert("No Such Event found", AlertType.warning);
                return RedirectToAction(nameof(Index), nameof(County));
            }

            if (_svc.DeleteCalendar(id))
            {
                Alert($"Event '{calendar.Title}' Successfully Deleted!", AlertType.info);
            }
            else
            {
                Alert("Event Could not be deleted", AlertType.warning);
            }
            return RedirectToAction(nameof(County), new { Id = calendar.CountyId });
        }


        [AcceptVerbs("GET", "POST")]
        public IActionResult ValidateDate([BindRequired, FromQuery] CalendarViewModel vm)
        {
            if (!_svc.IsValidCalendar(vm.ToCalendar()))
            {
                return Json("This event overlaps with an existing event.");
            }

            return Json(true);
        }
    }
}