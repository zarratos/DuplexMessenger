using System;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using MessengerInterfaces;

namespace MessengerClientWinForm
{
    public partial class FMessenger : Form
    {
        public static IMessengerService Server;
        private static DuplexChannelFactory<IMessengerService> channelFactory;

        // give the mutex a  unique name
        private const string MutexName = "##||ThisApp||##";
        // declare the mutex
        private readonly Mutex mutex;

        public string UserName { get; set; }

        private static FMessenger instance;
        public static FMessenger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FMessenger();
                }
                return instance;
            }
        }

        bool createdNew;
        public FMessenger()
        {
            InitializeComponent();

            channelFactory = new DuplexChannelFactory<IMessengerService>(new ClientCallback(), "MessageServiceEndPoint");
            Server = channelFactory.CreateChannel();
            UserName = Environment.UserName;
            mutex = new Mutex(true, MutexName, out createdNew);
            if (!createdNew)
            {
                // if the mutex already exists, notify and quit
                //                MessageBox.Show("This program is already running");
                //                Application.Current.Shutdown(0);
                UserName = "Vasya";
            }

            Server.Login(UserName);

            instance = this;
        }

        public void TakeMessage(string message, string userName)
        {
            var constructedMessage = string.Format("\n{0}: {1}", userName, message);
            //            var textRange = new TextRange(TextDisplayRichTextBox.Document.ContentEnd, TextDisplayRichTextBox.Document.ContentEnd)
            //            {
            //                Text = "textToColorize"
            //            };
            //            
            //            textRange.ApplyPropertyValue(TextElement.ForegroundProperty, t ? Brushes.Blue : Brushes.Red);
            rtbTextArea.AppendText(constructedMessage);
//            Application.Current.MainWindow.Activate();
        }

        private void bSend_Click(object sender, EventArgs e)
        {
            SendMessageToAll();
        }

        private void SendMessageToAll()
        {
            Server.SendMessageToAll(tbSendText.Text, UserName, Level.Minor);
            TakeMessage(tbSendText.Text, UserName);
            
        }
    }
}
