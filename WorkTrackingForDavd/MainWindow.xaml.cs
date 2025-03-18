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
using System.Runtime.CompilerServices;

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
    
    #endregion
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }
    
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
    
    private void StopTask(int indexOfSender)
    {
        string endTime = DateTime.Now.ToString("HH:mm:ss");
        switch (indexOfSender)
        {
            case 1:
                Index--;
                string startTime = BlockOneTime.Split(" ")[1];
                TimeSpan startTimeSpan = TimeSpan.Parse(startTime);
                TimeSpan endTimeSpan = TimeSpan.Parse(endTime);
                MessageBox.Show($"Start time {startTime}, End time {endTime} \n {endTimeSpan - startTimeSpan } Duration");
                

                BlockOneVisibility = BlockTwoVisibility;
                BlockOneTime = BlockTwoTime;
                BlockOneTitle = BlockTwoTitle;

                if (BlockTwoVisibility == Visibility.Hidden)
                {
                    BlockOneVisibility = Visibility.Hidden;
                    return;
                }

                BlockTwoVisibility = BlockThreeVisibility;
                BlockTwoTime = BlockThreeTime;
                BlockTwoTitle = BlockThreeTitle;

                BlockThreeVisibility = Visibility.Hidden;
                break;
            case 2:
                Index--;
                
                BlockTwoVisibility = BlockThreeVisibility;
                BlockTwoTime = BlockThreeTime;
                BlockTwoTitle = BlockThreeTitle;

                BlockThreeVisibility = Visibility.Hidden;
                break;
            case 3:
                BlockThreeVisibility = Visibility.Hidden;
                break;
            default:
                break;
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}