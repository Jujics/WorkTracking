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
using System.Runtime.CompilerServices;

namespace WorkTrackingForDavd;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    //private int taskNumber;
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
        MessageBox.Show($"User input: {userInput} at {startTime}");
    }
    
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}