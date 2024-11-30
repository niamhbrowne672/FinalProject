using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Data.Entities;
using SQLitePCL;

namespace FinalProject.Web.Models.CalendarModels
{
    public class CalendarViewModel
    {
        // non editable by user
        public int Id { get; set; }
        public int CountyId { get; set; }
        public string CountyName { get; set; }
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }

        [Remote(action: "ValidateDate", controller: "Calendar", AdditionalFields = "Id,Start,StartTime,End,EndTime")]
        public string Start { get; set; }
        public string StartTime { get; set; }

        [Remote(action: "ValidateDate", controller: "Calendar", AdditionalFields = "Id,Start,StartTime,End,EndTime")]
        public string End { get; set; }
        public string EndTime { get; set; }

        public IList<Calendar> Calendars { get; set; } = new List<Calendar>();

        // generate calendar event json for user
        public string SerializeCalendarsForUser(int userId, bool isAdmin)
        {
            var transformed = Calendars.Select(c =>
                new
                {
                    id = c.Id,
                    userId = c.UserId,
                    countyId = c.CountyId,
                    title = c.Title,              
                    location = c.Location,  
                    start = c.Start.ToString("yyyy-MM-dd HH:mm"),                  // format start date into string                   
                    end = c.End.ToString("yyyy-MM-dd HH:mm"),                      // format end date into string
                    url = $"/calendar/edit/{c.Id}",                                         // url to navigate to when event clicked
                    classNames = userId == c.UserId ? "owner" : "other",                 // style based on ownership (also done via calendar config property eventClassNames: ),
                }
            ).ToList();
            return JsonSerializer.Serialize(transformed);
        }

        // ---------------------- Mappers --------------------
        // make an viewmodel from a calendar entity
        public static CalendarViewModel FromCalendar(Calendar c)
        {
            return new CalendarViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Location = c.Location,
                Start = c.Start.ToString("yyyy-MM-dd"),
                StartTime = c.Start.ToString("HH:mm"),
                End = c.End.ToString("yyyy-MM-dd"),
                EndTime = c.End.ToString("HH:mm"),
                UserId = c.UserId,
                CountyId = c.CountyId,
            };
        }

        // Create a calendar entity from a ViewModel
        public Calendar ToCalendar()
        {
            return new Calendar
            {
                Id = this.Id,
                Title = this.Title,
                Location = this.Location,
                Start = DateTime.Parse($"{this.Start} {this.StartTime}"),
                //Start = DateTime.ParseExact($"{this.Start}T{this.StartTime}","yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-UK")),
                End = DateTime.Parse($"{this.End} {this.EndTime}"),
                //End = DateTime.ParseExact($"{this.End}T{this.EndTime}","yyyy-MM-ddTHH:mm:ss", new CultureInfo("en-UK")),
                UserId = this.UserId,
                CountyId = this.CountyId
            };
        }
    }
}