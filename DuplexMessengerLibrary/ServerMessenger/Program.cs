using System;
using System.ServiceModel;

using ServerMessenger;

namespace MEssengerServer
{
    class Program
    {
        public static MessengerService Server;
        static void Main(string[] args)
        {
            Server = new MessengerService();
            using (var host = new ServiceHost(Server))
            {
                host.Open();
                Console.WriteLine("Server is running...");
                Console.ReadLine();
            }
        }
    }
}
