using System.Diagnostics;
using System.Windows;

namespace WorkTrackingForDavd
{
    public partial class App : Application
    {
        private SystemTrayManager _trayManager;

        protected override void OnStartup(StartupEventArgs e)
        {
            var processName = Process.GetCurrentProcess().ProcessName;
            if (Process.GetProcessesByName(processName).Length > 1)
            {
                MessageBox.Show("Application is already running");
                Current.Shutdown();
                return;
            }
            
            base.OnStartup(e);
            CalSearch.StoreNumberEvent();
            var mainWindow = new MainWindow();
            _trayManager = new SystemTrayManager(mainWindow);
            mainWindow.Closing += (s, args) =>
            {
                args.Cancel = true;
                mainWindow.Hide();
            };
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _trayManager?.Dispose();
            base.OnExit(e);
        }
    }
}