using System;
using System.ServiceModel;

namespace MessengerInterfaces
{
    public interface IClient
    {
        [OperationContract(IsOneWay = true)]
        void GetMessage(string message, string userName, Level level, DateTime time);

        [OperationContract(IsOneWay = true)]
        void Ping();
    }
}
