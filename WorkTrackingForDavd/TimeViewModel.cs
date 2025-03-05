using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System;
using System.ComponentModel;

namespace WorkTrackingForDavd;

public class TimeViewModel : INotifyPropertyChanged
{
    private DateTime _currentTime;
    private DispatcherTimer _timer;

    public TimeViewModel()
    {
        _currentTime = DateTime.Now;
        
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        CurrentTime = DateTime.Now;
    }

    public DateTime CurrentTime
    {
        get { return _currentTime; }
        set
        {
            _currentTime = value;
            OnPropertyChanged(nameof(CurrentTimeString));
        }
    }

    public String CurrentTimeString
    {
        get { return _currentTime.ToString("HH:mm:ss"); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(String propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}