using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MessengerClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var wnd = new MainWindow { Title = "Messenger", ShowInTaskbar = false, ShowActivated = true, Topmost = true};
            wnd.ShowDialog();
        }
    }
}
