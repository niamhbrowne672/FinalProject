using FinalProject.Data.Entities;

namespace FinalProject.Data.Services;

public interface ICalendarService
{

    void Initialise();
    Calendar GetCalendar(int id);

    IList<Calendar> GetCalendars();
    IList<Calendar> GetCalendarsByCounty(int countyId);

    Calendar AddCalendar(Calendar calendar);
    bool DeleteCalendar(int id);
    Calendar UpdateCalendar(Calendar updated);

    IEnumerable<Calendar> GetCalendarsQuery(Func<Calendar, bool> query);
    bool IsValidCalendar(Calendar calendar);

    //county related operations
    IList<County> GetCounties();
    County GetCounty(int id);
    County AddCounty(County county);
}