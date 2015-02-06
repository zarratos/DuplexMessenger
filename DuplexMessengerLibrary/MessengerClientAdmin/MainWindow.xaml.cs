using System;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using MessengerInterfaces;

namespace MessengerClientAdmin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IMessengerService Server;
        private static DuplexChannelFactory<IMessengerService> channelFactory;

        // give the mutex a  unique name
        private const string MutexName = "##||ThisApp||##";
        // declare the mutex
        private readonly Mutex mutex;

        public string UserName { get; set; }

        bool createdNew;
        public MainWindow()
        {
            InitializeComponent();
            channelFactory = new DuplexChannelFactory<IMessengerService>(new ClientCallback(), "MessageServiceEndPoint");
            Server = channelFactory.CreateChannel();
            UserName = Environment.UserName;
            mutex = new Mutex(true, MutexName, out createdNew);
            if (!createdNew)
            {
                // Show opened App
                MessageBox.Show("This program is already running");
                Application.Current.Shutdown(0);
            }

            Server.Login(UserName);
        }

        public void TakeMessage(string message, string userName, Level level)
        {
            var constructedMessage = string.Format("\n{0}: {1}", userName, message);
            //            var textRange = new TextRange(TextDisplayRichTextBox.Document.ContentEnd, TextDisplayRichTextBox.Document.ContentEnd)
            //            {
            //                Text = "textToColorize"
            //            };
            //            
            //            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, t ? Brushes.Blue : Brushes.Red);
            TextDisplayRichTextBox.AppendText(constructedMessage);
            Application.Current.MainWindow.Activate();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessageToAll(Level.Minor);
        }

        private void SendMessageToAll(Level level)
        {
            Server.SendMessageToAll(MessageTextBox.Text, UserName, level);
            TakeMessage(MessageTextBox.Text, UserName, level);
            MessageTextBox.Clear();
        }

        private void MessageTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessageToAll(Level.Minor);
                Server.RemoveClient(UserName);
            }
        }
    }
}
