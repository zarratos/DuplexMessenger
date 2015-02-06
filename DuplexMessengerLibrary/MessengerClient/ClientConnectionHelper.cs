using System;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;
using MessengerInterfaces;
using Microsoft.Win32;

namespace MessengerClient
{
    internal class ClientConnectionHelper
    {
        private static IMessengerService _server;
        private static DuplexChannelFactory<IMessengerService> _channelFactory;
        private string _userName;
        private bool _isPingWorking = true;
        public delegate void EventHandler(object sender, EventArgs args);
        public event EventHandler MessageIsCome = delegate { };
        public string TextArea { get; set; }

        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_userName))
                    _userName = Environment.UserName;
                return _userName;
            }
        }

        private static ClientConnectionHelper _instance;
        public static ClientConnectionHelper Instance
        {
            get { return _instance ?? (_instance = new ClientConnectionHelper()); }
        }

        internal void Login()
        {
            var login = new Thread(LoginToServer);
            login.Start();
        }

        private void LoginToServer()
        {
            try
            {
                _channelFactory = new DuplexChannelFactory<IMessengerService>(new ClientCallback(),
                    "NetTcpBinding_MessageServiceEndPoint");
                _server = _channelFactory.CreateChannel();
                _server.Login(UserName);
            }
            catch (Exception ex)
            {
//                MessageBox.Show("Сервер не найден \n" + ex);
            }
        }

        internal void StartPinger()
        {
            var pinger = new Thread(PingTasker);
            pinger.Start();
        }

        private void PingTasker()
        {
            while (_isPingWorking)
            {
                if (!Ping())
                    try
                    {

                        LoginToServer();

                    }
                    catch (Exception)
                    {
                    }
                Thread.Sleep(10000);
            }
        }

        internal bool Ping()
        {
            var result = false;
            try
            {
                result = _server != null && string.Equals(_server.PingMessenger(), "success");
                
            }
            catch (Exception)
            {
            }
            return result;
        }

        internal void StopPinger()
        {
            _isPingWorking = false;
        }

        internal string SetEncodingForMessage(string message)
        {
            if (message.Contains("!DOCTYPE") || (TextArea != null && TextArea.Contains("!DOCTYPE"))) return message;
            var result = string.Format(@"<!DOCTYPE html ><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'>{0}", message);
            return result;
        }

        internal void DisconnectFromServer()
        {
            try
            {
                if (_server != null) _server.RemoveClient(UserName);
            }
            catch (Exception)
            {
            }
        }

        internal void SendMessageInThread(string richText)
        {
            var send = new Thread(() => SendMessageToAll(richText, Level.Minor, DateTime.Now));
            send.Start();
//            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => SendMessageToAll(richText, Level.Minor)));
        }

        private void SendMessageToAll(string message, Level level, DateTime time)
        {
            try
            {
                _server.SendMessageToAll(message, UserName, level, time);
            }
            catch (Exception)
            {
                LoginToServer();
                SendMessageToAll(message, level, time);
            }
        }

        internal void Dispose()
        {
            try
            {
                _userName = null;
                _channelFactory = null;
                _server = null;
            }
            catch (Exception)
            {
            }
        }
        
        internal void TakeMessage(string message, string userName, Level level, DateTime time)
        {
            TextArea += string.Format("<font color=\"red\">[{0}] {1}:</font><br>{2}<br>", time, userName, message);

            TextArea = SetEncodingForMessage(TextArea);

            MessageIsCome(this, new EventArgs());
        }

        internal string FormatMessage(string message, string userName, DateTime time)
        {
            var result = string.Format("<font color=\"blue\">[{0}] {1}:</font><br>{2}<br>", time, userName, message); 
            result = SetEncodingForMessage(result);
            return result;
        }

        internal void RegisterInStartup(bool isChecked)
        {
            try
            {
                var registryKey = Registry.LocalMachine.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (isChecked)
                {
                    if (registryKey != null) registryKey.SetValue("Messenger", Application.ExecutablePath);
                }
                else
                {
                    if (registryKey != null) registryKey.DeleteValue("Messenger");
                }
            }
            catch (Exception)
            {
            }
        }
    }
}