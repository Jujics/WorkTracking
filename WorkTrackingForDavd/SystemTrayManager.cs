using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Hardcodet.Wpf.TaskbarNotification;

namespace WorkTrackingForDavd
{
    public class SystemTrayManager : IDisposable
    {
        private readonly TaskbarIcon _trayIcon;
        private Window _mainWindow;

        public SystemTrayManager(Window mainWindow)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));

            var iconSource = LoadIcon();

            _trayIcon = new TaskbarIcon
            {
                IconSource = iconSource,
                ToolTipText = "Work Tracking for Davd",
                ContextMenu = CreateContextMenu()
            };

            _trayIcon.DoubleClickCommand = new RelayCommand(ShowMainWindow);
            _mainWindow.StateChanged += OnMainWindowStateChanged;
    
            _mainWindow.Show();
        }

        private ImageSource LoadIcon()
        {
            try
            {
                var uri = new Uri("pack://application:,,,/Resources/app_icon.ico", UriKind.Absolute);
                return new BitmapImage(uri);
            }
            catch
            {
                try
                {
                    var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    var exeDir = Path.GetDirectoryName(exePath);
                    var iconPath = Path.Combine(exeDir, "Resources", "app_icon.ico");
                    
                    if (File.Exists(iconPath))
                    {
                        return new BitmapImage(new Uri(iconPath));
                    }
                }
                catch { }
            }

            return new BitmapImage();
        }

        private void OnMainWindowStateChanged(object sender, EventArgs e)
        {
            if (_mainWindow.WindowState == WindowState.Minimized)
            {
                _mainWindow.Hide();
            }
        }

        private ContextMenu CreateContextMenu()
        {
            var menu = new ContextMenu();
            
            var openItem = new MenuItem { 
                Header = "Open",
                Command = new RelayCommand(ShowMainWindow)
            };
            
            var exitItem = new MenuItem {
                Header = "Exit",
                Command = new RelayCommand(ExitApplication)
            };

            menu.Items.Add(openItem);
            menu.Items.Add(exitItem);
            
            return menu;
        }

        private void ShowMainWindow()
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                _mainWindow.Show();
                _mainWindow.WindowState = WindowState.Normal;
                _mainWindow.Activate();
            });
        }

        private void ExitApplication()
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                Application.Current.Shutdown();
            });
        }

        public void Dispose()
        {
            _trayIcon?.Dispose();
            if (_mainWindow != null)
            {
                _mainWindow.StateChanged -= OnMainWindowStateChanged;
            }
            GC.SuppressFinalize(this);
        }
    }
}