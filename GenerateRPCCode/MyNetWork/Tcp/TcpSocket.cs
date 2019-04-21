
using NetWorkInterface;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyNetWork.Tcp
{
    public class TcpSocket : ISocket
    {
        Socket m_Socket;

        public TcpSocket(Socket socket)
        {
            m_Socket = socket;
        }

        public async Task<int> RecvAsync(ArraySegment<byte> seg)
        {
            int iRecvBytes = await m_Socket.ReceiveAsync(seg, SocketFlags.None);

            return iRecvBytes;
        }

        public async Task<int> SendAsync(ArraySegment<byte> seg)
        {
            int iSendBytes = await m_Socket.SendAsync(seg, SocketFlags.None);

            return iSendBytes;
        }
    }
}
