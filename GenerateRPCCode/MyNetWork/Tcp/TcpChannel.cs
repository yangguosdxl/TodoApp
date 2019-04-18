using CoolRpcInterface;
using NetWorkInterface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyNetWork.Tcp
{
    public class TcpChannel : IChannel, IProtocolUnit
    {
        Socket m_oSocket;
        
        CancellationTokenSource m_CTS = new CancellationTokenSource();

        ArraySegment<byte> m_RecvBuffer = new ArraySegment<byte>();

        ConcurrentQueue<ArraySegment<byte>> m_SendQueue = new ConcurrentQueue<ArraySegment<byte>>();

        public event Action ConnectedEvent;
        public event Action DisconnectedEvent;

        IMessageParser m_MessageParser;

        public TcpChannel(Socket socket)
        {
            m_oSocket = socket;

            byte[] buffer = new byte[512];
            m_RecvBuffer = new ArraySegment<byte>(buffer);
        }

        public void Start()
        {
            Task.Factory.StartNew(RecvAsync, m_CTS.Token, m_CTS.Token, TaskCreationOptions.None, TaskScheduler.Default);
            Task.Factory.StartNew(SendAsync, m_CTS.Token, m_CTS.Token, TaskCreationOptions.None, TaskScheduler.Default);
        }

        private async Task RecvAsync(object state)
        {
            CancellationToken cancelToken = (CancellationToken)state;
            while (true)
            {
                cancelToken.ThrowIfCancellationRequested();

                int iRecvBytes = await m_oSocket.ReceiveAsync(m_RecvBuffer, SocketFlags.None);

                if (iRecvBytes > 0)
                    m_MessageParser.Process(m_RecvBuffer);
            }
        }

        private async Task SendAsync(object state)
        {
            CancellationToken cancelToken = (CancellationToken)state;
            while (true)
            {
                cancelToken.ThrowIfCancellationRequested();

                ArraySegment<byte> seg;
                if (m_SendQueue.TryDequeue(out seg))
                {
                    int iSendBytes = await m_oSocket.SendAsync(seg, SocketFlags.None);
                }
                else
                {
                    // @todo wait notfiy
                }
            }
        }

        public void Send(byte[] buffer, int start, int len)
        {
            byte[] bytes = new byte[len];
            Array.Copy(buffer, start, bytes, 0, len);

            ArraySegment<byte> seg = new ArraySegment<byte>(bytes, 0, len);

            m_SendQueue.Enqueue(seg);

            // @todo notify send task;
        }

        public Task SendAsync(byte[] buff, int start, int len)
        {
            throw new NotImplementedException();
        }

        public Task<(byte[], int, int)> RecvAsync()
        {
            throw new NotImplementedException();
        }

        public (byte[], int, int) Recv()
        {
            throw new NotImplementedException();
        }

        public (byte[], int start, int len) Process(byte[] buff, int start, int len)
        {
            throw new NotImplementedException();
        }
    }
}
