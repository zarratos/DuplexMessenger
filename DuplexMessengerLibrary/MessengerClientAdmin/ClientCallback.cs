using System;
using System.ServiceModel;
using System.Windows;
using MessengerInterfaces;

namespace MessengerClientAdmin
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    class ClientCallback : IClient
    {
        public void GetMessage(string message, string userName, Level level, DateTime time)
        {
            ((MainWindow)Application.Current.MainWindow).TakeMessage(message, userName, level);
        }

        public void Ping()
        {
        }
    }
}
