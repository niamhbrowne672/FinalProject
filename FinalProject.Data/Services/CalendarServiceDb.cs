using FinalProject.Data.Entities;
using FinalProject.Data.Repositories;
using Microsoft.EntityFrameworkCore;


namespace FinalProject.Data.Services;

public class CalendarServiceDb : ICalendarService
{
    private readonly DatabaseContext _ctx;

    public CalendarServiceDb(DatabaseContext ctx)
    {
        _ctx = ctx;
    }

    public void Initialise()
    {
        _ctx.Initialise();
    }


    // ------------------ Calendar Related Operations ------------------------

    // retrieve list of all Events
    public IList<Calendar> GetCalendars()
    {
        return _ctx.Calendars.ToList();
    }

    // Retrive Event by Id 
    public Calendar GetCalendar(int id)
    {
        return _ctx.Calendars.FirstOrDefault(c => c.Id == id);
    }

    // Return calendars for county
    public IList<Calendar> GetCalendarsByCounty(int countyId)
    {
        return _ctx.Calendars.Where(c => c.CountyId == countyId).ToList();
    }

    public bool IsValidCalendar(Calendar calendar)
    {
        if (calendar.Start > calendar.End)
        {
            return false;
        }

        var overlappingCount = _ctx.Calendars.Count(c =>
            c.Id != calendar.Id &&
            c.CountyId == calendar.CountyId &&
            calendar.Start < c.End &&
            calendar.End > c.Start);

        Console.WriteLine($"Overlap Count: {overlappingCount}");
        return overlappingCount == 0;
    }

    public Calendar AddCalendar(Calendar calendar)
    {
        try
        {
            // Validate the calendar object
            if (!IsValidCalendar(calendar))
            {
                Console.WriteLine($"Calendar is not valid: Title={calendar.Title}, Start={calendar.Start}, End={calendar.End}");
                return null;
            }

            // Explicitly set Id to 0 to ensure it's treated as a new record
            var newCalendar = new Calendar
            {
                Id = 0, // Ensures the database assigns a new Id
                Title = calendar.Title,
                Location = calendar.Location,
                Start = calendar.Start,
                End = calendar.End,
                CountyId = calendar.CountyId,
                UserId = calendar.UserId
            };

            // Add the new calendar to the database
            Console.WriteLine($"Adding calendar: Title={newCalendar.Title}, Start={newCalendar.Start}, End={newCalendar.End}, CountyId={newCalendar.CountyId}");
            _ctx.Calendars.Add(newCalendar);
            _ctx.SaveChanges(); // Write to database
            Console.WriteLine($"Calendar added successfully with Id={newCalendar.Id}");
            return newCalendar;
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error adding calendar: {ex.Message}");
            if (calendar != null)
            {
                Console.WriteLine($"Calendar details: Id={calendar.Id}, Title={calendar.Title}, Start={calendar.Start}, End={calendar.End}");
            }
            return null;
        }
    }

    public bool DeleteCalendar(int id)
    {
        var calendar = GetCalendar(id);
        if (calendar == null)
        {
            return false;
        }
        _ctx.Calendars.Remove(calendar);
        _ctx.SaveChanges(); // write to database
        return true;
    }

    public Calendar UpdateCalendar(Calendar updated)
    {
        // verify the Event exists
        var calendar = GetCalendar(updated.Id);
        if (calendar == null || !IsValidCalendar(updated))
        {
            return null;
        }

        // update the details of the Event retrieved and save
        calendar.Title = updated.Title;
        calendar.Location = updated.Location;
        calendar.Start = updated.Start;
        calendar.End = updated.End;
        calendar.CountyId = updated.CountyId;
        calendar.UserId = updated.UserId;

        _ctx.SaveChanges(); // write to database
        return calendar;
    }

    public IEnumerable<Calendar> GetCalendarsQuery(Func<Calendar, bool> query)
    {
        return _ctx.Calendars.Where(query);
    }

    // ---------------------- County Related Operations ------------------------

    public IList<County> GetCounties()
    {
        return _ctx.Counties.ToList();
    }

    public County GetCounty(int id)
    {
        return _ctx.Counties.FirstOrDefault(c => c.Id == id);
    }

    public County AddCounty(County county)
    {
        if (_ctx.Counties.Any(c => c.Name == county.Name))
        {
            return null;
        }

        _ctx.Counties.Add(county);
        _ctx.SaveChanges();
        return county;
    }
}