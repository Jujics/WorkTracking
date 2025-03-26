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
        var targetEnd = targetDate.Date;
        return (calendarEvent.Start.Value >= targetStart && calendarEvent.Start.Value < targetEnd) || 
               (calendarEvent.End.Value > targetStart && calendarEvent.End.Value <= targetEnd) || 
               (calendarEvent.Start.Value <= targetStart && calendarEvent.End.Value >= targetEnd);
    }

    private void OnCloseView(object sender, RoutedEventArgs e)
    {
        if (_navigationFrame != null && _navigationFrame.CanGoBack)
        {
            _navigationFrame.GoBack();
        }
        else if (NavigationService != null && NavigationService.CanGoBack)
        {
            NavigationService.GoBack();
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