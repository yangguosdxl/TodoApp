using Cool.Coroutine;
using CoolRpcInterface;
using CSRPC;
using MyNetWork;
using NetWorkInterface;
using System;

using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ClientTest
{
    class CallAsync : ICallAsync
    {
        ISocketTask m_Socket;

        IMessageEncoder m_MessageCoder = new MessageEncoder();

        WaitCompleteTasks m_WaitCompleteTasks = new WaitCompleteTasks(1024);

        ProtocolHandler[] m_ProtocoHandlers = new ProtocolHandler[(int)ProtoID.COUNT];
        ProtocolDeserializer[] m_ProtocolDeserializers = new ProtocolDeserializer[(int)ProtoID.COUNT];

        ConcurrentQueue<(int, int, IMessage)> m_RecvMessages = new ConcurrentQueue<(int, int, IMessage)>();

        public CallAsync(string ip, int port, NetType netType)
        {
            DefaultSocketConnector socketConnector = new DefaultSocketConnector();
            m_Socket = socketConnector.Connect(ip, port, netType);

            if (m_Socket == null)
                throw new Exception($"failed connect to socket {ip}:{port}:{netType}");

            m_Socket.OnMessage += OnMessage;
            m_Socket.OnDisconnect += OnDisconnect;
        }

        private void OnDisconnect()
        {
            throw new NotImplementedException();
        }

        private void OnMessage(int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len)
        {
            IMessage msg = m_ProtocolDeserializers[iProtocolID](messageBuff, start, len);
            m_RecvMessages.Enqueue((iProtocolID, iCommunicateID, msg));
        }

        public void Update()
        {
            ValueTuple<int, int, IMessage> msg;
            while(m_RecvMessages.TryDequeue(out msg))
            {
                int iProtocolID = msg.Item1;
                int iCommunicateID = msg.Item2;
                IMessage message = msg.Item3;

                if (iCommunicateID != 0)
                {
                    m_WaitCompleteTasks.OnComplete(iCommunicateID, ref message);
                }
                else
                {
                    if (iProtocolID >= 0 && iProtocolID < (int)ProtoID.COUNT)
                    {
                        ProtocolHandler h = m_ProtocoHandlers[iProtocolID];
                        h(iCommunicateID, message);
                    }
                }
            }
        }

        public void AddProtocolHandler(int iProtoID, ProtocolHandler h)
        {
            m_ProtocoHandlers[iProtoID] = h;
        }

        public void AddProtocolDeserializer(int iProtoID, ProtocolDeserializer deserializer)
        {
            m_ProtocolDeserializers[iProtoID] = deserializer;
        }



        public void SendWithoutResponse(int iChunkType, int iCommunicateID, int iProtoID, Func<byte[], int, (byte[], int, int)> action)
        {
            m_Socket.Send(iChunkType, iCommunicateID, iProtoID, action);
        }

        public MyTask<T> SendWithResponse<T>(int iChunkType, int iProtoID, Func<byte[], int, (byte[], int, int)> action)
        {
            WaitCompleteTask<T> task = m_WaitCompleteTasks.WaitComplete<T>();

            m_Socket.Send(iChunkType, task.ID, iProtoID, action);

            return task;
        }

        public void SendWithoutResponse(int iChunkType, int iCommunicateID, int iProtoID, byte[] bytes, int iStart, int len)
        {
            throw new NotImplementedException();
        }

        public Task<(byte[], int, int)> SendWithResponse(int iChunkType, int iProtoID, byte[] bytes, int iStart, int len)
        {
            throw new NotImplementedException();
        }
    }

}
