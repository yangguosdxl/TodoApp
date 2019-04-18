using NetWorkInterface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNetWork.Tcp
{
    public class TcpListener : INetWorkListener
    {
        Socket m_oSocketListener;
        CancellationTokenSource m_CancelTS = new CancellationTokenSource();
        public void Init(EndPoint ep)
        {
            m_oSocketListener = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            m_oSocketListener.NoDelay = true;
            //绑定端口
            m_oSocketListener.Bind(ep);
            //挂起的连接队列的最大长度。
            m_oSocketListener.Listen(1000);

            Task.Factory.StartNew(Accept, m_CancelTS.Token, m_CancelTS.Token, TaskCreationOptions.None, TaskScheduler.Default);
        }

        public async Task Accept(object state)
        {
            CancellationToken cancelTocken = (CancellationToken)state;
            while (true)
            {
                cancelTocken.ThrowIfCancellationRequested();

                Socket socket = await m_oSocketListener.AcceptAsync();

                
            }
        }
    }
}
