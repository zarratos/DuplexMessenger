using System;
using System.Collections.Concurrent;
using System.ServiceModel;

namespace MessengerInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMessengerService" in both code and config file together.
    [ServiceContract(CallbackContract = typeof(IClient))]
    public interface IMessengerService
    {
        [OperationContract]
        string PingMessenger();

        [OperationContract]
        int Login(string userName);

        [OperationContract]
        void SendMessageToAll(string message, string userName, Level level, DateTime time);

        [OperationContract]
        void RemoveClient(string userName);

        [OperationContract]
        void PrintActiveClients();
    }
}
