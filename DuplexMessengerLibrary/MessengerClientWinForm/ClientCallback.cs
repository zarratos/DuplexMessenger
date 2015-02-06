using System;
using System.ServiceModel;
using MessengerInterfaces;

namespace MessengerClientWinForm
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    class ClientCallback : IClient
    {
        public void GetMessage(string message, string userName, Level level)
        {
            FMessenger.Instance.BeginInvoke(new Action(() => FMessenger.Instance.TakeMessage(message, userName)));
        }

        public void CloseSession()
        {
            FMessenger.Instance.Close();
        }
    }
}
