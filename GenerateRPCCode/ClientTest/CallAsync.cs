using Cool.Coroutine;
using Cool.Interface.Rpc;
using CSRPC;
using Cool.NetWork;
using Cool.Interface.NetWork;
using System;

using System.Collections.Concurrent;
using System.Threading.Tasks;
using Cool.CSCommon;
using System.Diagnostics.Contracts;

namespace ClientTest
{
    class CallAsync : ICallAsync
    {
        ISocketTask m_Socket;

        WaitCompleteTasks m_WaitCompleteTasks = new WaitCompleteTasks(1024);

        ProtocolHandler[] m_ProtocoHandlers = new ProtocolHandler[RpcServiceHelper.ProtoCount];
        ProtocolDeserializer[] m_ProtocolDeserializers = new ProtocolDeserializer[RpcServiceHelper.ProtoCount];

        ConcurrentQueue<(int, int, IMessage)> m_RecvMessages = new ConcurrentQueue<(int, int, IMessage)>();

        ICoolRpc[] m_aCoolRpcs = new ICoolRpc[RpcServiceHelper.RpcServiceCount];
        IRPCHandlerMap[] m_aRpcHandlerMaps = new IRPCHandlerMap[RpcServiceHelper.RpcServiceCount];

        public CallAsync(string ip, int port, NetType netType)
        {
            DefaultSocketConnector socketConnector = new DefaultSocketConnector();
            m_Socket = socketConnector.Connect(ip, port, netType);
            m_Socket.MessageEncoder = new MessageEncoder();
            m_Socket.MessageDecoder = new MessageDecoder();

            if (m_Socket == null)
                throw new Exception($"failed connect to socket {ip}:{port}:{netType}");

            m_Socket.OnMessage += OnMessage;
            m_Socket.OnDisconnect += OnDisconnect;

            m_Socket.Startup();
        }

        private void OnDisconnect()
        {
            throw new NotImplementedException();
        }

        public void OnMessage(int iChunkType, int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len)
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

                if (iCommunicateID != 0 && NetHelper.IsResponseCommunicateID(iCommunicateID))
                {
                    m_WaitCompleteTasks.OnComplete(iCommunicateID, ref message);
                }
                else
                {
                    if (iProtocolID >= 0 && iProtocolID < RpcServiceHelper.ProtoCount)
                    {
                        ProtocolHandler h = m_ProtocoHandlers[iProtocolID];
                        if (iCommunicateID != 0)
                            iCommunicateID = NetHelper.ConvertToResponseCommunicateID(iCommunicateID);
                        
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

        public MyTask<IMessage> SendWithResponse(int iChunkType, int iProtoID, Func<byte[], int, (byte[], int, int)> action)
        {
            WaitCompleteTask<IMessage> task = m_WaitCompleteTasks.WaitComplete<IMessage>();
            int iCommunicateID = NetHelper.ConvertToRequestCommunicateID(task.ID);
            m_Socket.Send(iChunkType, iCommunicateID, iProtoID, action);

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

        public T GetRpc<T>() where T : ICoolRpc
        {
            ICoolRpc rpc = m_aCoolRpcs[RpcServiceHelper.GetID<T>()];
            if (rpc == null)
            {
                rpc = RpcServiceHelper.CreateRpc<T>();
                rpc.Init(new Serializer(), this, 0);

                m_aCoolRpcs[RpcServiceHelper.GetID<T>()] = rpc;
            }

            return (T)rpc;
        }

        public void AddRpcHandlers<T>(ICoolRpc rpcHandler) where T : ICoolRpc
        {
            rpcHandler.Init(new Serializer(), this, 0);

            int id = RpcServiceHelper.GetID<T>();
            Contract.Ensures(rpcHandler != null && m_aRpcHandlerMaps[id] == null);

            IRPCHandlerMap handlerMap = RpcServiceHelper.CreateRpcHandlerMap<T>(rpcHandler);

            m_aRpcHandlerMaps[id] = handlerMap;
        }
    }

}
