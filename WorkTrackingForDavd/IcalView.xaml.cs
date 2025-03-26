using System.IO;
using System.Windows.Controls;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net;
using Calendar = Ical.Net.Calendar;

namespace WorkTrackingForDavd;

public partial class IcalView : Page
{
    public IcalView()
    {
        InitializeComponent();
    }

    public static List<CalendarEventCl> CalendarEvents(string icalFilePath, DateTime targetDate)
    {
        var events = new List<CalendarEventCl>();

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
                events.Add(new CalendarEventCl
                {
                    Summary = calendarEvent.Summary,
                    Start = calendarEvent.Start.Value,
                    End = calendarEvent.End.Value,
                });
            }
        }
        return events.OrderBy(e => e.Start).ToList();
    }

    private static bool IsEventOnDate(CalendarEvent calendarEvent, DateTime targetDate)
    {
        var targetStart = targetDate.Date;
        var targetEnd = targetDate.Date;
        return (calendarEvent.Start.Value >= targetStart && calendarEvent.Start.Value < targetEnd) || 
               (calendarEvent.End.Value > targetStart && calendarEvent.End.Value <= targetEnd) || 
               (calendarEvent.Start.Value <= targetStart && calendarEvent.End.Value >= targetEnd);
    }

    private void OnCloseView 
    {
        //todo
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public class CalendarEventCl
    {
        public string Summary { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

}