using CoolRpcInterface;
using NetWorkInterface;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MyNetWork
{
    public class DefaultSocket : ISocketTask
    {
        ISocket m_Socket;
        
        CancellationTokenSource m_CTS = new CancellationTokenSource();

        byte[] m_RecvBuffer;

        ConcurrentQueue<ArraySegment<byte>> m_SendQueue = new ConcurrentQueue<ArraySegment<byte>>();

        public event Action OnDisconnect;
        public event Action<int, int, byte[], int, int> OnMessage;

        //TaskCompletionSource<bool> m_SendEvent = new TaskCompletionSource<bool>(false);

        IMessageCoder m_MessageParser;

        public IMessageCoder MessageParser
        {
            get => m_MessageParser;
            set
            {
                m_MessageParser = value;

                m_MessageParser.OnMessage += _OnMessage;
            }
        }

        private void _OnMessage(int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len)
        {
            OnMessage(iProtocolID, iCommunicateID, messageBuff, start, len);
        }

        public DefaultSocket(ISocket socket)
        {
            m_Socket = socket;

            m_RecvBuffer = new byte[NetworkConfig.MESSAGE_MAX_BYTES];
        }  

        public void Startup()
        {
            Task.Factory.StartNew(RecvLoopAsync, m_CTS.Token, m_CTS.Token, TaskCreationOptions.None, TaskScheduler.Default);
            Task.Factory.StartNew(SendLoopAsync, m_CTS.Token, m_CTS.Token, TaskCreationOptions.None, TaskScheduler.Default);
        }

        public void Send(byte[] buffer, int start, int len)
        {
            byte[] bytes = new byte[len];
            Array.Copy(buffer, start, bytes, 0, len);

            ArraySegment<byte> seg = new ArraySegment<byte>(bytes, 0, len);

            m_SendQueue.Enqueue(seg);

            // @todo notify send task;
            //m_SendEvent.SetResult(true);
        }

        private async Task RecvLoopAsync(object state)
        {
            CancellationToken cancelToken = (CancellationToken)state;
            while (true)
            {
                cancelToken.ThrowIfCancellationRequested();

                ArraySegment<byte> seg = new ArraySegment<byte>(m_RecvBuffer);

                int iRecvBytes = await m_Socket.RecvAsync(seg);

                if (iRecvBytes > 0)
                    m_MessageParser.Decode(m_RecvBuffer, 0, iRecvBytes);
            }
        }

        private async Task SendLoopAsync(object state)
        {
            CancellationToken cancelToken = (CancellationToken)state;
            while (true)
            {
                cancelToken.ThrowIfCancellationRequested();

                ArraySegment<byte> seg;
                if (m_SendQueue.TryDequeue(out seg))
                {
                    int iSendBytes = await m_Socket.SendAsync(seg);
                    if (iSendBytes != seg.Count)
                    {
                        // @todo what happened
                    }
                }
                else
                {
                    // @todo wait notfiy

                    await Task.Delay(1);
                }
            }
        }
    }
}
