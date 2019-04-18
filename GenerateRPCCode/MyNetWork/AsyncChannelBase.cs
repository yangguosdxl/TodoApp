using CoolRpcInterface;
using NetWorkInterface;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyNetWork
{
    abstract class AsyncChannelBase : IChannel, IMessageParser
    {
        Socket m_oSocket;

        CancellationTokenSource m_CTS = new CancellationTokenSource();

        ArraySegment<byte> m_RecvBuffer = new ArraySegment<byte>();

        ConcurrentQueue<ArraySegment<byte>> m_SendQueue = new ConcurrentQueue<ArraySegment<byte>>();

        public event Action ConnectedEvent;
        public event Action DisconnectedEvent;

        IMessageParser m_MessageParser;

        public AsyncChannelBase(Socket socket)
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

        protected abstract Task RecvAsync(object state);

        protected abstract Task SendAsync(object state);
        

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
