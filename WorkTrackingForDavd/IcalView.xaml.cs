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

public partial class IcalView : Page
{
    
    private Frame _navigationFrame;
    public IcalView(Frame navigationFrame = null)
    {
        InitializeComponent();
        _navigationFrame = navigationFrame;
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
        var targetEnd = targetDate.Date.AddDays(1);
    
        
        var eventStart = calendarEvent.Start.Value;
        var eventEnd = calendarEvent.End.Value;
    
        // For all-day events, iCal.Net sets time to 00:00:00
        if (calendarEvent.IsAllDay)
        {
            return eventStart.Date <= targetDate.Date && eventEnd.Date > targetDate.Date;
        }
    
        return (eventStart >= targetStart && eventStart < targetEnd) || 
               (eventEnd > targetStart && eventEnd <= targetEnd) || 
               (eventStart <= targetStart && eventEnd >= targetEnd);
    }

    private void OnCloseView(object sender, RoutedEventArgs e)
    {
        
        NavigationService?.Navigate(null);
    
        NavigationService?.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
    
        _navigationFrame?.Navigate(null);
    }
    private void OnChoose(object sender, RoutedEventArgs e)
    {
        if (DatePicker.SelectedDate.HasValue)
        {
            DateTime selectedDate = DatePicker.SelectedDate.Value;
            string icalFilePath = "TasksCalendar.ics";
            var events = CalendarEvents(icalFilePath, selectedDate);
            try
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine($"Events on {selectedDate.ToShortDateString()}:");
        
                if (events.Count == 0)
                {
                    message.AppendLine("No events found for this date.");
                }
                else
                {
                    foreach (var evt in events)
                    {
                        message.AppendLine($"- {evt.Summary}");
                        message.AppendLine($"  From: {evt.Start.ToShortTimeString()} To: {evt.End.ToShortTimeString()}");
                        message.AppendLine();
                    }
                }

                MessageBox.Show(message.ToString(), "Calendar Events");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }
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