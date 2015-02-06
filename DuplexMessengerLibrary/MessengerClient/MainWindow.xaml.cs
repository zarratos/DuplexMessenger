using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;
using mshtml;
using Application = System.Windows.Application;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using WebBrowser = System.Windows.Controls.WebBrowser;

namespace MessengerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Fields

        private const string MutexName = "##||Messenger||##";
        bool createdNew;
        private Mutex mutex;
        private NotifyIcon m_notifyIcon;
        private WindowState m_storedWindowState = WindowState.Normal;
        #endregion Fields

        #region Properties
        public string TextArea { get; set; }

       
        #endregion Properties

        public MainWindow()
        {
            InitializeComponent();
            ClientConnectionHelper.Instance.RegisterInStartup(true);
            
            mutex = new Mutex(true, MutexName, out createdNew);
            if (!createdNew)
            {
                System.Windows.MessageBox.Show("This program is already running");
                Application.Current.Shutdown(0);
            }

            ClientConnectionHelper.Instance.Login();
            ClientConnectionHelper.Instance.StartPinger();
            MoveInTray();
            ClientConnectionHelper.Instance.MessageIsCome += (sender, args) => TakeMessage();
        }
     
        #region NotifyIcon

        private void MoveInTray()
        {
            var streamResourceInfo = Application.GetResourceStream(new Uri(Icon.ToString()));
            if (streamResourceInfo != null)
            {
                var iconStream = streamResourceInfo.Stream;
                var icon = new Icon(iconStream);

                m_notifyIcon = new NotifyIcon
                {
                    Icon = icon,
                };
            }

            SetNotifyMoveInTray();
            m_notifyIcon.Click += m_notifyIcon_Click;
            m_notifyIcon.Visible = true;
        }

        private void m_notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = m_storedWindowState;
        }

        void OnStateChanged(object sender, EventArgs args)
        {
            FlashWindow.Flash(this);
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (m_notifyIcon != null)
                    m_notifyIcon.ShowBalloonTip(2000);
            }
            else
            {
                m_storedWindowState = WindowState;
                SetWindowOnRightBottom();
            }
        }

        private void ShowNotifyNewMessage()
        {
            if (WindowState != WindowState.Minimized) return;
            m_notifyIcon.BalloonTipText = Properties.Resources.MainWindow_ShowNotifyNewMessage_TipText;
            m_notifyIcon.BalloonTipTitle = Properties.Resources.MainWindow_ShowNotifyNewMessage_Title;
            m_notifyIcon.Text = Properties.Resources.MainWindow_ShowNotifyNewMessage_Text;

            if (m_notifyIcon != null)
                m_notifyIcon.ShowBalloonTip(int.MaxValue);

            SetNotifyMoveInTray();
        }

        private void SetNotifyMoveInTray()
        {
            m_notifyIcon.BalloonTipText = Properties.Resources.MainWindow_MoveInTray_Messenger_Moved_To_Tray;
            m_notifyIcon.BalloonTipTitle = Properties.Resources.MainWindow_MoveInTray_Messenger;
            m_notifyIcon.Text = Properties.Resources.MainWindow_MoveInTray_Messenger;
        }

        #endregion NotifyIcon

        #region FormEvents

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageTextBox.Document.Blocks.Count == 0) return;
            var richText = GetSendTextFromControl();
            MessageTextBox.Document.Blocks.Clear();
            ClientConnectionHelper.Instance.TextArea += ClientConnectionHelper.Instance.FormatMessage(richText,
                    Environment.UserName, DateTime.Now);
            TakeMessage();
//            TakeMessage(richText, ClientConnectionHelper.Instance.UserName, Level.Minor, DateTime.Now);
            ClientConnectionHelper.Instance.SendMessageInThread(richText);
        }

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (MessageTextBox.Document.Blocks.Count == 0) return;
                ScrollChatDown(WebBrowserArea);
                var richText = GetSendTextFromControl();
                ClientConnectionHelper.Instance.SendMessageInThread(richText);
                ClientConnectionHelper.Instance.TextArea += ClientConnectionHelper.Instance.FormatMessage(richText,
                    Environment.UserName, DateTime.Now);
                MessageTextBox.Document.Blocks.Clear();
                TakeMessage();
//                TakeMessage(richText, ClientConnectionHelper.Instance.UserName, Level.Minor, DateTime.Now);
                ScrollChatDown(WebBrowserArea);
            }
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            WindowState = WindowState.Minimized;
            e.Cancel = true;
        }

        private void DisposeClient()
        {
            m_notifyIcon.Dispose();
            m_notifyIcon = null;
            ClientConnectionHelper.Instance.DisconnectFromServer();
            ClientConnectionHelper.Instance.StopPinger();
            ClientConnectionHelper.Instance.Dispose();
        }

        #endregion FormEvents

        #region PrivateHelpers

        public void TakeMessage()
        {
            Dispatcher.Invoke(PutMessageOnChat);
        }

        private void PutMessageOnChat()
        {
            ShowNotifyNewMessage();
            var constructedMessage = ClientConnectionHelper.Instance.TextArea;
            ShowWindowFromTray();
            WebBrowserArea.NavigateToString(constructedMessage);
            ScrollChatDown(WebBrowserArea);
        }

        private void ShowWindowFromTray()
        {
            Show();
            WindowState = WindowState.Normal;
            this.Visibility = Visibility.Visible;
            Activate();
        }

        private string GetSendTextFromControl()
        {
            return new TextRange(MessageTextBox.Document.ContentStart, MessageTextBox.Document.ContentEnd).Text;
        }

        private void ScrollChatDown(WebBrowser webBrowserArea)
        {
            var html = webBrowserArea.Document as HTMLDocument;
            if (html != null) html.parentWindow.scroll(0, 1000000000);
        }

        #endregion PrivateHelpers

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            SetWindowOnRightBottom();
        }

        private void SetWindowOnRightBottom()
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        private void WebBrowserArea_OnNavigated(object sender, NavigationEventArgs e)
        {
            ScrollChatDown(WebBrowserArea);
        }
    }
}
