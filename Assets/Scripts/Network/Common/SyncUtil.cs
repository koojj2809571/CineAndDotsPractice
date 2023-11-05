using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Network
{
    public class SyncUtil
    {
        public static Socket CreateSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        
        public static void ConSync(ref Socket socket,string ip, int port)
        {
            socket.Connect(ip, port);
        } 

        public static void SendText(string content, Socket socket)
        {
            var bytes = Encoding.UTF8.GetBytes(content);
            socket.Send(bytes);
            
        }

        public static IEnumerator WaiServer(Socket socket)
        {
            var tempBuffer = new byte[1024];
            yield return 0;
            socket.Receive(tempBuffer);
            var msg = Encoding.UTF8.GetString(tempBuffer);
            Console.WriteLine($"服务器已记录：{msg}");
            yield return null;
        }

        public static void Close(Socket socket)
        {
            socket.Close();
        }

    }
}