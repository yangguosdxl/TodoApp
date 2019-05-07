
using NetWorkInterface;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyNetWork.Tcp
{
    public class TcpAcceptor : ISocketAcceptor
    {
        Socket m_Socket;

        public TcpAcceptor(EndPoint ep)
        {
            m_Socket = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_Socket.NoDelay = true;
            //绑定端口
            m_Socket.Bind(ep);
            //挂起的连接队列的最大长度。
            m_Socket.Listen(1000);
        }

        public async Task<ISocket> AcceptAsync()
        {
            Socket socket = await m_Socket.AcceptAsync();

            return new TcpSocket(socket);
        }
    }
}
