
using NetWorkInterface;
using System.Net;
using System.Net.Sockets;

namespace MyNetWork.Tcp
{
    public class TcpConnector : ISocketConnector
    {
        public ISocket Connect(EndPoint endPoint)
        {
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(endPoint);
                if (socket.Connected)
                    return new TcpSocket(socket);
            }
            finally
            {
                if (socket == null || socket.Connected == false)
                    socket.Dispose();
            }

            return null;
        }

        public ISocket Connect(string host, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(host, port);

                if (socket.Connected)
                    return new TcpSocket(socket);
            }
            finally
            {
                if (socket == null || socket.Connected == false)
                    socket.Dispose();
            }

            return null;
        }
    }
}
