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
        BlockOneVisibility = Visibility.Visible;
        BlockOneTime = "Debut de la tache " + startTime;
        BlockOneTitle = userInput;
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}