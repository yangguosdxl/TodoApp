
using NetWorkInterface;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyNetWork.Kcp
{
    public class KcpSocket : ISocket
    {
        Socket m_Socket;

        public KcpSocket(Socket socket)
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
