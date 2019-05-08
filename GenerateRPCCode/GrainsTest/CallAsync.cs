using Cool.Coroutine;
using CoolRpcInterface;
using CSRPC;
using GrainInterface;
using MyNetWork;
using NetWorkInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrainsTest
{
    class CallAsync : ICallAsync
    {
        ProtocolDeserializer[] m_ProtocolDeserializers = new ProtocolDeserializer[(int)ProtoID.COUNT];
        ProtocolHandler[] m_ProtocolHandlers = new ProtocolHandler[(int)ProtoID.COUNT];

        IClientSessionGrain m_ClientSessionGrain;

        byte[] m_SendBuffer = new byte[NetworkConfig.MESSAGE_MAX_BYTES];

        WaitCompleteTasks m_WaitCompleteTasks = new WaitCompleteTasks(1024);
        IMessageEncoder m_MessageEncoder = new MessageEncoder();

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

        public void OnMessage(int iChunkType, int iProtocolID, int iCommunicateID, byte[] messageBuff, int start, int len)
        {
            ProtocolDeserializer protocolDeserializer = m_ProtocolDeserializers[iProtocolID];
            IMessage message = protocolDeserializer(messageBuff, start, len);


            if (iCommunicateID != 0)
            {
                m_WaitCompleteTasks.OnComplete(iCommunicateID, ref message);
            }
            else
            {
                if (iProtocolID >= 0 && iProtocolID < (int)ProtoID.COUNT)
                {
                    ProtocolHandler h = m_ProtocolHandlers[iProtocolID];
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

            m_ClientSessionGrain.Send(iProtoID, task.ID, bytes, start, len);

            return task;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
