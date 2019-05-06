using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;

namespace CSRPC
{
    public class ISHelloService_HandlerMap : IRPCHandlerMap
    {
        private RpcTestInterface.ISHelloService m_service;

        public ISHelloService_HandlerMap(RpcTestInterface.ISHelloService service)
        {
            m_service = service;
            service.CallAsync.AddProtocolHandler((int)ProtoID.EISHelloService_Hello_MsgIn, Process_Hello);
            service.CallAsync.AddProtocolDeserializer((int)ProtoID.EISHelloService_Hello_MsgIn, Deserialize_Hello);
            service.CallAsync.AddProtocolHandler((int)ProtoID.EISHelloService_HelloInt_MsgIn, Process_HelloInt);
            service.CallAsync.AddProtocolDeserializer((int)ProtoID.EISHelloService_HelloInt_MsgIn, Deserialize_HelloInt);
            service.CallAsync.AddProtocolHandler((int)ProtoID.EISHelloService_Hello2_MsgIn, Process_Hello2);
            service.CallAsync.AddProtocolDeserializer((int)ProtoID.EISHelloService_Hello2_MsgIn, Deserialize_Hello2);
            service.CallAsync.AddProtocolHandler((int)ProtoID.EISHelloService_Hello3_MsgIn, Process_Hello3);
            service.CallAsync.AddProtocolDeserializer((int)ProtoID.EISHelloService_Hello3_MsgIn, Deserialize_Hello3);
        }

        private async MyTask Process_Hello(int iCommunicateID, IMessage _msg)
        {
            ISHelloService_Hello_MsgIn msg = (ISHelloService_Hello_MsgIn)_msg;
            m_service.Hello();
        }

        private IMessage Deserialize_Hello(byte[] bytes, int iStartIndex, int iCount)
        {
            ISHelloService_Hello_MsgIn msg = m_service.Serializer.Deserialize<ISHelloService_Hello_MsgIn>(bytes, iStartIndex, iCount);
            return msg;
        }

        private async MyTask Process_HelloInt(int iCommunicateID, IMessage _msg)
        {
            ISHelloService_HelloInt_MsgIn msg = (ISHelloService_HelloInt_MsgIn)_msg;
            var ret = await m_service.HelloInt(msg.a);
            ISHelloService_HelloInt_MsgOut msgRet = new ISHelloService_HelloInt_MsgOut();
            msgRet.Value = ret;
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = m_service.Serializer.Serialize(msgRet, buffer, start);
                return msgSerializeInfo;
            };
            m_service.CallAsync.SendWithoutResponse(m_service.ChunkType, iCommunicateID, (int)ProtoID.EISHelloService_HelloInt_MsgOut, f);
        }

        private IMessage Deserialize_HelloInt(byte[] bytes, int iStartIndex, int iCount)
        {
            ISHelloService_HelloInt_MsgIn msg = m_service.Serializer.Deserialize<ISHelloService_HelloInt_MsgIn>(bytes, iStartIndex, iCount);
            return msg;
        }

        private async MyTask Process_Hello2(int iCommunicateID, IMessage _msg)
        {
            ISHelloService_Hello2_MsgIn msg = (ISHelloService_Hello2_MsgIn)_msg;
            m_service.Hello2(msg.p);
        }

        private IMessage Deserialize_Hello2(byte[] bytes, int iStartIndex, int iCount)
        {
            ISHelloService_Hello2_MsgIn msg = m_service.Serializer.Deserialize<ISHelloService_Hello2_MsgIn>(bytes, iStartIndex, iCount);
            return msg;
        }

        private async MyTask Process_Hello3(int iCommunicateID, IMessage _msg)
        {
            ISHelloService_Hello3_MsgIn msg = (ISHelloService_Hello3_MsgIn)_msg;
            var ret = await m_service.Hello3(msg.p);
            ISHelloService_Hello3_MsgOut msgRet = new ISHelloService_Hello3_MsgOut();
            msgRet.Value = ret;
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = m_service.Serializer.Serialize(msgRet, buffer, start);
                return msgSerializeInfo;
            };
            m_service.CallAsync.SendWithoutResponse(m_service.ChunkType, iCommunicateID, (int)ProtoID.EISHelloService_Hello3_MsgOut, f);
        }

        private IMessage Deserialize_Hello3(byte[] bytes, int iStartIndex, int iCount)
        {
            ISHelloService_Hello3_MsgIn msg = m_service.Serializer.Deserialize<ISHelloService_Hello3_MsgIn>(bytes, iStartIndex, iCount);
            return msg;
        }
    }
}
