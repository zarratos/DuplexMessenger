using System;
using System.Collections.Concurrent;
using System.Linq;
using System.ServiceModel;
using MessengerInterfaces;

namespace MessengerServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class MessengerService : IMessengerService
    {
        public ConcurrentDictionary<string, ConnectedClient> ConnectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        public string PingMessenger()
        {
            return "success";
        }

        public int Login(string userName)
        {
            if (ConnectedClients.Any(client => String.Equals(client.Key, userName, StringComparison.CurrentCultureIgnoreCase)))
            {
                Console.WriteLine("[{0}] user: {1} is already logged in", DateTime.Now, userName);
                return 1;
            }
            var estabilishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            var newClient = new ConnectedClient {Connection = estabilishedUserConnection, UserName = userName};

            ConnectedClients.TryAdd(userName, newClient);
            Console.WriteLine("[{0}] user: {1} logged in!", DateTime.Now, userName);
           return 0;
        }

        public void SendMessageToAll(string message, string userName, Level level, DateTime time)
        {
            Console.WriteLine("[{0}] {1}: sent message to all", DateTime.Now, userName);
            if (!ConnectedClients.ContainsKey(userName))
                Login(userName);
            foreach (var client in ConnectedClients.Where(client => !String.Equals(client.Key, userName, StringComparison.CurrentCultureIgnoreCase)))
            {
                try
                {
                    client.Value.Connection.GetMessage(message, userName, level, time);
                }
                catch (Exception)
                {
                    ConnectedClient value;
                    ConnectedClients.TryRemove(client.Key, out value);
                }
            }
            Console.WriteLine("[{0}] {1}: message to all sended", DateTime.Now, userName);
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
                    Console.WriteLine("[{0}] Disconnected: {1}", DateTime.Now, client.Key);
                }
            }
            catch (Exception)
            {
            }
        }

        public void PrintActiveClients()
        {
            foreach (var client in ConnectedClients)
            {
                try
                {
                    Console.WriteLine("\n {0}", client.Key);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
