using NetWorkInterface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MyNetWork.Tcp
{
    public class TcpConnector : ISocketConnector
    {
        public ISocket Connect(EndPoint endPoint)
        {
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);

            if (socket.Connected)
                return new TcpSocket(socket);

            return null;
        }

        public ISocket Connect(string host, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(host, port);

            if (socket.Connected)
                return new TcpSocket(socket);

            return null;
        }
    }
}
