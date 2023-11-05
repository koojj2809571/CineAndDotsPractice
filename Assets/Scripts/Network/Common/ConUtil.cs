using System.Net.Sockets;

// ReSharper disable once CheckNamespace
namespace Network
{
    public class ConUtil
    {
        public static Socket CreateSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        
    }
}