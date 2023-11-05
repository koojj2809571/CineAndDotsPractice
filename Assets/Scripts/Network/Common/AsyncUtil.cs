using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Network
{
    public class AsyncUtil
    {
        
        private static readonly byte[] _receiveBuffer = new byte[1024];
        private static byte[] _sendBuffer = new byte[1024];
        public static void ConAsync(Socket socket, string ip, int port)
        {
            socket.BeginConnect(ip, port, OnConOk, socket);
        }

        public static void Receive(Socket socket)
        {
            socket.BeginReceive(_receiveBuffer, 0, 1024, SocketFlags.None, OnReceive, socket);
        }

        public static void Send(Socket socket, string sendText)
        {
            _sendBuffer = Encoding.UTF8.GetBytes(sendText);
            socket.BeginSend(_sendBuffer, 0, _sendBuffer.Length, SocketFlags.None, OnSend, socket);

        }

        public static void OnConOk(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("连接成功");
            Send(socket, "连接成功");
            Receive(socket);
        }

        public static void OnReceive(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            socket.EndReceive(ar);
            var receiveStr = Encoding.UTF8.GetString(_receiveBuffer);
            Debug.Log(receiveStr);
            Receive(socket);
        }
        
        public static void OnSend(IAsyncResult ar)
        {
            var socket = (Socket)ar.AsyncState;
            socket.EndSend(ar);
        }
    }
}