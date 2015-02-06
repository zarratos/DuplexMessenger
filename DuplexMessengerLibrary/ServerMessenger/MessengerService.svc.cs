using System;
using System.Collections.Concurrent;
using System.ServiceModel;
using MessengerInterfaces;

namespace ServerMessenger
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class MessengerService : IMessengerService
    {
        public ConcurrentDictionary<string, ConnectedClient> ConnectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        public int Login(string userName)
        {
            //             Если уже кто то залогинился с этим именем проверяем
            foreach (var client in ConnectedClients)
            {
                if (String.Equals(client.Key, userName, StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine(string.Format("user: {0} is already logged in"));
                    return 1;
                }
            }
            var estabilishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            var newClient = new ConnectedClient { Connection = estabilishedUserConnection, UserName = userName };

            ConnectedClients.TryAdd(userName, newClient);
            Console.WriteLine(string.Format("user: {0} logged in!"));
            return 0;
        }

        public void SendMessageToAll(string message, string userName, Level level)
        {
            foreach (var client in ConnectedClients)
            {
                if (!String.Equals(client.Key, userName, StringComparison.CurrentCultureIgnoreCase))
                {
                    try
                    {
                        client.Value.Connection.GetMessage(message, userName, level);
                    }
                    catch (Exception)
                    {
                        ConnectedClient value;
                        ConnectedClients.TryRemove(client.Key, out value);
                    }

                }
            }
        }

        public void RemoveClient(string userName)
        {
            try
            {
                foreach (var client in ConnectedClients)
                {
                    if (!String.Equals(client.Key, userName, StringComparison.CurrentCultureIgnoreCase)) continue;
                    ConnectedClient value;
                    ConnectedClients.TryRemove(client.Key, out value);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
