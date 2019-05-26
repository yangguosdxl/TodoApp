using Cool.Coroutine;
using Cool.Interface.Rpc;
using CSRPC;
using GrainInterface;
using Cool.NetWork;
using Cool.Interface.NetWork;
using System;
using System.Collections.Generic;
using System.Text;
using Cool.CSCommon;
using System.Diagnostics.Contracts;

namespace GrainsTest
{
    public class CallAsync : ICallAsync
    {
        ProtocolDeserializer[] m_ProtocolDeserializers = new ProtocolDeserializer[RpcServiceHelper.ProtoCount];
        ProtocolHandler[] m_ProtocolHandlers = new ProtocolHandler[RpcServiceHelper.ProtoCount];

        IClientSessionGrain m_ClientSessionGrain;

        byte[] m_SendBuffer = new byte[NetworkConfig.MESSAGE_MAX_BYTES];

        WaitCompleteTasks m_WaitCompleteTasks = new WaitCompleteTasks(1024);
        IMessageEncoder m_MessageEncoder = new MessageEncoder();

        ICoolRpc[] m_aCoolRpcs = new ICoolRpc[RpcServiceHelper.RpcServiceCount];

        IRPCHandlerMap[] m_aRpcHandlerMaps = new IRPCHandlerMap[RpcServiceHelper.RpcServiceCount];

        public CallAsync(IClientSessionGrain clientSessionGrain)
        {
            m_ClientSessionGrain = clientSessionGrain;
        }

        public void AddProtocolDeserializer(int iProtoID, ProtocolDeserializer deserializer)
        {
            m_ProtocolDeserializers[iProtoID] = deserializer;
        }

        public void AddProtocolHandler(int iProtoID, ProtocolHandler h)
        {
            m_ProtocolHandlers[iProtoID] = h;
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

        public void OnMessage(int iChunkType, int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len)
        {
            ProtocolDeserializer protocolDeserializer = m_ProtocolDeserializers[iProtocolID];
            IMessage message = protocolDeserializer(messageBuff, start, len);


            if (NetHelper.IsValidCommunicateID(iCommunicateID) && NetHelper.IsResponseCommunicateID(iCommunicateID))
            {
                m_WaitCompleteTasks.OnComplete(iCommunicateID, ref message);
            }
            else
            {
                if (iProtocolID >= 0 && iProtocolID < RpcServiceHelper.ProtoCount)
                {
                    ProtocolHandler h = m_ProtocolHandlers[iProtocolID];
                    if (NetHelper.IsValidCommunicateID(iCommunicateID))
                        iCommunicateID = NetHelper.ConvertToResponseCommunicateID(iCommunicateID);

                    h(iCommunicateID, message);
                }
            }
        }

        public void SendWithoutResponse(int iChunkType, int iCommunicateID, int iProtoID, byte[] bytes, int iStart, int len)
        {
            throw new NotImplementedException();
        }

        public void SendWithoutResponse(int iChunkType, int iCommunicateID, int iProtoID, Func<byte[], int, (byte[], int, int)> action)
        {
            var (bytes, start, len) = action(m_SendBuffer, 0);
            m_ClientSessionGrain.Send(iProtoID, iCommunicateID, bytes, start, len);
        }

        public System.Threading.Tasks.Task<(byte[], int, int)> SendWithResponse(int iChunkType, int iProtoID, byte[] bytes, int iStart, int len)
        {
            throw new NotImplementedException();
        }

        public Cool.Coroutine.MyTask<IMessage> SendWithResponse(int iChunkType, int iProtoID, Func<byte[], int, (byte[], int, int)> action)
        {
            var (bytes, start, len) = action(m_SendBuffer, 0);

            WaitCompleteTask<IMessage> task = m_WaitCompleteTasks.WaitComplete<IMessage>();

            int iCommunicateID = NetHelper.ConvertToRequestCommunicateID(task.ID);

            m_ClientSessionGrain.Send(iProtoID, iCommunicateID, bytes, start, len);

            return task;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
