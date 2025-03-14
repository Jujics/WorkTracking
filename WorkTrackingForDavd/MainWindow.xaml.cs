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

namespace WorkTrackingForDavd;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void Start_Task(object sender, RoutedEventArgs e)
    {
        // Access the TextBox's text
        string userInput = InputTask.Text;
        string startTime = DateTime.Now.ToString("HH:mm:ss");
        // Do something with the user input
        MessageBox.Show($"User input: {userInput} at {startTime}");
    }
    
}