using MessengerInterfaces;

namespace MessengerServer
{
    public class ConnectedClient
    {
        public IClient Connection;
        
        public string UserName { get; set; }
    }
}
