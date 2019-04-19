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

        byte[] m_RecvBuffer;

        ConcurrentQueue<ArraySegment<byte>> m_SendQueue = new ConcurrentQueue<ArraySegment<byte>>();

        public event Action ConnectedEvent;
        public event Action DisconnectedEvent;

        IMessageParser m_MessageParser;

        ISerializer m_Serializer;

        public IMessageParser MessageParser
        {
            get => m_MessageParser;
            set
            {
                m_MessageParser = value;

                m_MessageParser.OnMessage += OnMessage;
            }
        }

        private void OnMessage(int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len)
        {
            m_Serializer.Deserialize<>
        }

        public TcpChannel(Socket socket)
        {
            m_oSocket = socket;

            m_RecvBuffer = new byte[NetworkConfig.MESSAGE_MAX_BYTES];
        }

        public void Start()
        {
            Task.Factory.StartNew(RecvAsync, m_CTS.Token, m_CTS.Token, TaskCreationOptions.None, TaskScheduler.Default);
            Task.Factory.StartNew(SendAsync, m_CTS.Token, m_CTS.Token, TaskCreationOptions.None, TaskScheduler.Default);
        }

        public void Send(byte[] buffer, int start, int len)
        {
            byte[] bytes = new byte[len];
            Array.Copy(buffer, start, bytes, 0, len);

            ArraySegment<byte> seg = new ArraySegment<byte>(bytes, 0, len);

            m_SendQueue.Enqueue(seg);

            // @todo notify send task;
        }

        private async Task RecvAsync(object state)
        {
            CancellationToken cancelToken = (CancellationToken)state;
            while (true)
            {
                cancelToken.ThrowIfCancellationRequested();

                ArraySegment<byte> seg = new ArraySegment<byte>(m_RecvBuffer);

                int iRecvBytes = await m_oSocket.ReceiveAsync(seg, SocketFlags.None);

                if (iRecvBytes > 0)
                    m_MessageParser.Process(m_RecvBuffer, 0, iRecvBytes);
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
                    if (iSendBytes != seg.Count)
                    {
                        // @todo what happened
                    }
                }
                else
                {
                    // @todo wait notfiy
                }
            }
        }

        public (byte[], int start, int len) Process(byte[] buff, int start, int len)
        {
            throw new NotImplementedException();
        }
    }
}
