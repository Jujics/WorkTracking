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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Calendar = Ical.Net.Calendar;
using Ical.Net.CalendarComponents;
using Path = System.IO.Path;

namespace WorkTrackingForDavd;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    #region Fields

    #region Visibility
    
    private Visibility _blockOneVisibility = Visibility.Hidden;
    public Visibility BlockOneVisibility
    {
        get => _blockOneVisibility;
        set
        {
            if (_blockOneVisibility != value)
            {
                _blockOneVisibility = value;
                OnPropertyChanged();
            }
        }
    }
    
    private Visibility _blockTwoVisibility = Visibility.Hidden;
    public Visibility BlockTwoVisibility
    {
        get => _blockTwoVisibility;
        set
        {
            if (_blockTwoVisibility != value)
            {
                _blockTwoVisibility = value;
                OnPropertyChanged();
            }
        }
    }
    
    private Visibility _blockThreeVisibility = Visibility.Hidden;
    public Visibility BlockThreeVisibility
    {
        get => _blockThreeVisibility;
        set
        {
            if (_blockThreeVisibility != value)
            {
                _blockThreeVisibility = value;
                OnPropertyChanged();
            }
        }
    }
    
    #endregion
    
    #region TimeString
    
    private String _blockOneTime = string.Empty;
    public String BlockOneTime
    {
        get => _blockOneTime;
        set
        {
            if (_blockOneTime != value)
            {
                _blockOneTime = value;
                OnPropertyChanged();
            }
        }
    }
    
    private String _blockTwoTime = string.Empty;
    public String BlockTwoTime
    {
        get => _blockTwoTime;
        set
        {
            if (_blockTwoTime != value)
            {
                _blockTwoTime = value;
                OnPropertyChanged();
            }
        }
    }
    
    private String _blockThreeTime = string.Empty;
    public String BlockThreeTime
    {
        get => _blockThreeTime;
        set
        {
           if (BlockThreeTime != value)
           {
               _blockThreeTime = value;
               OnPropertyChanged();
           }
        }
    }
    
    #endregion
    
    #region TitleString
    private String _blockOneTitle = string.Empty;
    public String BlockOneTitle
    {
        get => _blockOneTitle;
        set
        {
            if (_blockOneTitle != value)
            {
                _blockOneTitle = value;
                OnPropertyChanged();
            }
        }
    }
    
    private String _blockTwoTitle = string.Empty;
    public String BlockTwoTitle
    {
        get => _blockTwoTitle;
        set
        {
            if (_blockTwoTitle != value)
            {
                _blockTwoTitle = value;
                OnPropertyChanged();
            }
        }
    }
    
    private String _blockThreeTitle = string.Empty;
    public String BlockThreeTitle
    {
        get => _blockThreeTitle;
        set
        {
            if (_blockThreeTitle != value)
            {
                _blockThreeTitle = value;
                OnPropertyChanged();
            }
        }
    }
    #endregion

    private int _index = 0;
    public int Index
    {
        get => _index;
        set
        {
            if (_index != value)
            {
                _index = value;
            }
        }
    }

    private Calendar _calendar;
    
    #endregion

    #region OnStart
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        _calendar = new Calendar();
        _calendar.AddTimeZone("Europe/Paris");

        LoadCalendarFromFile();
    }

    private void LoadCalendarFromFile()
    {
        string filePath = "TasksCalendar.ics";

        if (File.Exists(filePath))
        {
            try
            {
                string fileContent = File.ReadAllText(filePath);
                _calendar = Calendar.Load(fileContent);
                if (_calendar == null)
                { 
                    throw new InvalidOperationException("Calendar could not be deserialized.");
                }
            }
            catch (Exception ex)
            {
                _calendar = new Calendar();
                _calendar.AddTimeZone("Europe/Paris");
                MessageBox.Show($"Failed to load calendar: {ex.Message}. A new calendar has been created.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            _calendar = new Calendar();
            _calendar.AddTimeZone("Europe/Paris");
        }
    }
    
    #endregion
    
    #region OnStop
    protected override void OnClosing(CancelEventArgs e)
    {
        SaveCalendarToFile();
        base.OnClosing(e);
    }
    
    #endregion
    
    #region ButtonClicEvents
    private void Start_Task(object sender, RoutedEventArgs e)
    {
        string userInput = InputTask.Text;
        string startTime = DateTime.Now.ToString("HH:mm:ss");
        switch (Index)
        {
            case 0 :
                BlockOneVisibility = Visibility.Visible;
                BlockOneTime = "Debut de la tache  " + startTime;
                BlockOneTitle = userInput;
                _index++;
                break;
            case 1 :
                BlockTwoVisibility = Visibility.Visible;
                BlockTwoTime = "Debut de la tache  " + startTime;
                BlockTwoTitle = userInput;
                _index++;
                break;
            case 2 :
                BlockThreeVisibility = Visibility.Visible;
                BlockThreeTime = "Debut de la tache  " + startTime;
                BlockThreeTitle = userInput;
                _index++;
                break;
            default:
                MessageBox.Show("Trop de taches a la fois");
                break;
        }
    }

    #region Stop task Args
    private void Stop_Task_One(object sender, RoutedEventArgs e)
    {
        StopTask(1);
    }
    private void Stop_Task_Two(object sender, RoutedEventArgs e)
    {
        StopTask(2);
    }
    private void Stop_Task_Three(object sender, RoutedEventArgs e)
    {
        StopTask(3);
    }
    #endregion
    
    private void StopTask(int taskIndex)
    {
        string endTime = DateTime.Now.ToString("HH:mm:ss");
        string startTime = string.Empty;
        TimeSpan duration = TimeSpan.Zero;
        switch (taskIndex)
        {
            case 1:
                startTime = BlockOneTime.Split(" ")[1];
                duration = DateTime.Parse(endTime) - DateTime.Parse(startTime);
                LogTask(BlockOneTitle, startTime, endTime, duration);
                ShiftTasks(1);
                break;
            case 2:
                startTime = BlockTwoTime.Split(" ")[1];
                duration = DateTime.Parse(endTime) - DateTime.Parse(startTime);
                LogTask(BlockTwoTitle, startTime, endTime, duration);
                ShiftTasks(2);
                break;
            case 3:
                startTime = BlockThreeTime.Split(" ")[1];
                duration = DateTime.Parse(endTime) - DateTime.Parse(startTime);
                LogTask(BlockThreeTitle, startTime, endTime, duration);
                BlockThreeVisibility = Visibility.Hidden;
                break;
        }
        MessageBox.Show($"Task {taskIndex} stopped.\nDuration: {duration:hh\\:mm\\:ss}");
    }

    private void ShiftTasks(int stoppedTaskIndex)
    {
        if (stoppedTaskIndex == 1)
        {
            BlockOneVisibility = BlockTwoVisibility;
            BlockOneTime = BlockTwoTime;
            BlockOneTitle = BlockTwoTitle;
            BlockTwoVisibility = BlockThreeVisibility;
            BlockTwoTime = BlockThreeTime;
            BlockTwoTitle = BlockThreeTitle;
            BlockThreeVisibility = Visibility.Hidden;
        }
        else if (stoppedTaskIndex == 2)
        {
            BlockTwoVisibility = BlockThreeVisibility;
            BlockTwoTime = BlockThreeTime;
            BlockTwoTitle = BlockThreeTitle;
            BlockThreeVisibility = Visibility.Hidden;
        }
        Index--;
    }
    
    private void SwitchPage(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new IcalView(MainFrame));
    }

    #endregion
    
    #region SaveCalendarToFile
    private void LogTask(string title, string startTime, string endTime, TimeSpan duration)
    {
        var calendarEvent = new CalendarEvent
        {
            Summary = title,
            Start = new CalDateTime(DateTime.Parse(startTime)),
            End = new CalDateTime(DateTime.Parse(endTime)),
        };
        _calendar.Events.Add(calendarEvent);
        SaveCalendarToFile();
    }
    private void SaveCalendarToFile()
    {
        var serializer = new Ical.Net.Serialization.CalendarSerializer();
        string icalString = serializer.SerializeToString(_calendar);
        
        string filePath = "TasksCalendar.ics";
        
        try
        {
            File.WriteAllText(filePath, icalString);
            MessageBox.Show($"Calendar saved to {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save calendar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    #endregion

    #region other button interaction

    private void OpenExplorer(object sender, RoutedEventArgs e)
    {
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
    
        string relativePath = "TasksCalendar.ics";
    
        string fullPath = Path.Combine(appDirectory, relativePath);
    
        // Verify file exists
        if (File.Exists(fullPath))
        {
            Process.Start("explorer.exe", $"/select,\"{fullPath}\"");
        }
        else
        {
            MessageBox.Show("File not found: " + fullPath);
        }
    }
    

    #endregion

    #region PropertyChange
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

}