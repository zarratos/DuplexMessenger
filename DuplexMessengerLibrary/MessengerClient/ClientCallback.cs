using System;
using System.ServiceModel;
using System.Windows;
using MessengerInterfaces;

namespace MessengerClient
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    class ClientCallback : IClient
    {
        public void GetMessage(string message, string userName, Level level, DateTime time)
        {
//            ((MainWindow)Application.Current.MainWindow).TakeMessage(message, userName, level, time);
            ClientConnectionHelper.Instance.TakeMessage(message, userName, level, time);
        }

        public void Ping()
        {
        }
    }
}
