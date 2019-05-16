using Cool;
using NetWorkInterface;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MyNetWork
{
    public class DefaultSocket : ISocketTask
    {
        ISocket m_Socket;
        
        CancellationTokenSource m_CTS = new CancellationTokenSource();

        byte[] m_RecvBuffer;

        byte[] m_SendBuffer;

        ConcurrentQueue<(int,int, int, Func<byte[], int, (byte[], int, int)>)> m_SendTaskQueue = new ConcurrentQueue<(int,int, int, Func<byte[], int, (byte[], int, int)>)>();

        public event Action OnDisconnect;
        public event Action<int, int, int, byte[], int, int> OnMessage;

        IMessageDecoder m_MessageDecoder;

        public IMessageDecoder MessageDecoder
        {
            get => m_MessageDecoder;
            set
            {
                m_MessageDecoder = value;

                m_MessageDecoder.OnMessage += _OnMessage;
            }
        }

        public IMessageEncoder MessageEncoder { get; set; }

        private void _OnMessage(int iChunkType, int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len)
        {
            OnMessage(iChunkType, iProtocolID, iCommunicateID, messageBuff, start, len);
        }

        public DefaultSocket(ISocket socket)
        {
            Debug.Assert(socket != null);
            m_Socket = socket;

            m_RecvBuffer = new byte[NetworkConfig.MESSAGE_MAX_BYTES];
            m_SendBuffer = new byte[NetworkConfig.MESSAGE_MAX_BYTES];
        }  

        public void Startup()
        {
            Task.Factory.StartNew(RecvLoopAsync, m_CTS.Token, m_CTS.Token, TaskCreationOptions.None, TaskScheduler.Default);
            Task.Factory.StartNew(SendTaskLoopAsync, m_CTS.Token, m_CTS.Token, TaskCreationOptions.None, TaskScheduler.Default);
        }

        private async Task RecvLoopAsync(object state)
        {
            CancellationToken cancelToken = (CancellationToken)state;
            while (true)
            {
                cancelToken.ThrowIfCancellationRequested();

                try
                {
                    ArraySegment<byte> seg = new ArraySegment<byte>(m_RecvBuffer);

                    int iRecvBytes = await m_Socket.RecvAsync(seg);

                    if (iRecvBytes > 0)
                        m_MessageDecoder.Decode(m_RecvBuffer, 0, iRecvBytes);
                }
                catch(SocketException e)
                {
                    m_Socket.Dispose();

                    Logger.Warn(e);

                    OnDisconnect();
                    
                    return;
                }
                catch(Exception e)
                {
                    Logger.Warn(e);
                }

            }
        }

        public void Send(int iChunkType, int iCommunicateID, int iProtoID, Func<byte[], int, (byte[], int,int)> f)
        {
            m_SendTaskQueue.Enqueue((iChunkType, iCommunicateID, iProtoID, f));
        }

        private async Task SendTaskLoopAsync(object state)
        {
            CancellationToken cancelToken = (CancellationToken)state;
            while (true)
            {
                cancelToken.ThrowIfCancellationRequested();

                try
                {
                    (int, int, int, Func<byte[], int, (byte[], int, int)>) sendTask;
                    if (m_SendTaskQueue.TryDequeue(out sendTask))
                    {
                        int iChunkType = sendTask.Item1;
                        int iCommunicateID = sendTask.Item2;
                        int iProtoID = sendTask.Item3;
                        Func<byte[], int, (byte[], int, int)> f = sendTask.Item4;
                        var (sendBytes, start, len) = f(m_SendBuffer, NetworkConfig.MESSAGE_HEAD_BYTES);

                        (sendBytes, start, len) = MessageEncoder.Encode(iChunkType, iCommunicateID, iProtoID, sendBytes, start, len);

                        ArraySegment<byte> seg = new ArraySegment<byte>(sendBytes, start, len);

                        int iSendBytes = await m_Socket.SendAsync(seg);
                        if (iSendBytes != seg.Count)
                        {
                            throw new Exception($"Real send bytes {iSendBytes}, Need send bytes {seg.Count}");
                        }
                    }
                    else
                    {
                        // @todo wait notfiy
                        await Task.Delay(1);
                    }
                }
                catch (SocketException e)
                {
                    m_Socket.Dispose();

                    Logger.Warn(e);

                    OnDisconnect();

                    return;
                }
                catch (Exception e)
                {
                    Logger.Warn(e);
                }
            }
        }
    }
}
