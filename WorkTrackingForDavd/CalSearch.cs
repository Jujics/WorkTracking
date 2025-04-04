using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Calendar = Ical.Net.Calendar;
using Ical.Net.CalendarComponents;

namespace WorkTrackingForDavd;

public class CalSearch
{
    public static List<int> eventListYear = new List<int>();

    public static void StoreNumberEvent()
    {
        int i = 0;
        eventListYear.Clear();
        
        for(DateTime date = DateTime.Today - TimeSpan.FromDays(370); date <= DateTime.Today; date = date.AddDays(1))
        {
            eventListYear.Add(CalendarEvents("TasksCalendar.ics", date));
            Console.WriteLine(eventListYear[i]);
            i++;
        }
    }
    
    public static int CalendarEvents(string icalFilePath, DateTime targetDate)
    {
        int howManyEventInDay = 0;

        if (!File.Exists(icalFilePath))
        {
            throw new FileNotFoundException("Could not find ical file", icalFilePath);
        }
        
        var calendar = Calendar.Load(File.ReadAllText(icalFilePath));
        var calendarEvents = calendar.Events;
        foreach (var calendarEvent in calendarEvents)
        {
            if (IsEventOnDate(calendarEvent, targetDate))
            {
                howManyEventInDay++;
            }
        }
        return howManyEventInDay;
    }
    
    private static bool IsEventOnDate(CalendarEvent calendarEvent, DateTime targetDate)
    {
        var targetStart = targetDate.Date;
        var targetEnd = targetDate.Date.AddDays(1);
    
        
        var eventStart = calendarEvent.Start.Value;
        var eventEnd = calendarEvent.End.Value;
        
        if (calendarEvent.IsAllDay)
        {
            return eventStart.Date <= targetDate.Date && eventEnd.Date > targetDate.Date;
        }
    
        return (eventStart >= targetStart && eventStart < targetEnd) || 
               (eventEnd > targetStart && eventEnd <= targetEnd) || 
               (eventStart <= targetStart && eventEnd >= targetEnd);
    }
    
}