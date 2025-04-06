using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Ical.Net.CalendarComponents;
using Calendar = Ical.Net.Calendar;

namespace WorkTrackingForDavd;

public partial class IcalView : Page
{
    
    private Frame _navigationFrame;
    public IcalView(Frame navigationFrame = null)
    {
        InitializeComponent();
        _navigationFrame = navigationFrame;
        GenerateColorBoxes();
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
    
    private void GenerateColorBoxes()
    {
        CalSearch.StoreNumberEvent();
        ColorBoxContainer.Children.Clear();
        for (int i = 0; i < 370; i++)
        {
            int daysAgo = 370 - i;
            int eventCount = CalSearch.eventListYear[i];
            Color boxColor = GetColorForEventCount(eventCount);
            DateTime currentDate = DateTime.Today.AddDays(-daysAgo);
        
            var tooltipContent = new StackPanel();
            tooltipContent.Children.Add(new TextBlock { 
                Text = currentDate.ToString("MMM dd, yyyy (dddd)"), 
                FontWeight = FontWeights.Bold 
            });
            tooltipContent.Children.Add(new TextBlock { 
                Text = $"Events: {eventCount}" 
            });
        
            Border box = new Border
            {
                Width = 10,
                Height = 10,
                Margin = new Thickness(1),
                Background = new SolidColorBrush(boxColor),
                ToolTip = tooltipContent,
                Cursor = Cursors.Hand
            };
            box.MouseEnter += (s, e) => box.Opacity = 0.7;
            box.MouseLeave += (s, e) => box.Opacity = 1.0;

            ColorBoxContainer.Children.Add(box);
        }
        GenerateTodayColorBoxes();
        
    }

    private void GenerateTodayColorBoxes()
    {
        var dayTooltipContent = new StackPanel();
        dayTooltipContent.Children.Add(new TextBlock { 
            Text = "Today", 
            FontWeight = FontWeights.Bold 
        });
        dayTooltipContent.Children.Add(new TextBlock { 
            Text = $"Events: {CalSearch.eventListYear[CalSearch.eventListYear.Count-1]}" 
        });
        
        Border tdBox = new Border
        {
            Width = 10,
            Height = 10,
            Margin = new Thickness(1),
            Background = new SolidColorBrush(GetColorForEventCount(CalSearch.eventListYear[CalSearch.eventListYear.Count-1])),
            ToolTip = dayTooltipContent,
            Cursor = Cursors.Hand
        };
        tdBox.MouseEnter += (s, e) => tdBox.Opacity = 0.7;
        tdBox.MouseLeave += (s, e) => tdBox.Opacity = 1.0;

        ColorBoxContainer.Children.Add(tdBox);
    }

    private Color GetColorForEventCount(int eventCount)
    {
        if (eventCount == 0)
            return Colors.LightGray;
        else if (eventCount == 1)
            return Colors.LightGreen;
        else if (eventCount == 2)
            return Colors.Green;
        else if (eventCount == 3)
            return Colors.GreenYellow;
        else if (eventCount == 4)
            return Colors.Orange;
        else
            return Colors.Red;
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