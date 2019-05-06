using CoolRpcInterface;
using CSRPC;
using GrainInterface;
using MyNetWork;
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

        public Cool.Coroutine.MyTask<T> SendWithResponse<T>(int iChunkType, int iProtoID, Func<byte[], int, (byte[], int, int)> action)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
