using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;

namespace CSRPC
{
    public class ICHelloService_HandlerMap : IRPCHandlerMap
    {
        private RpcTestInterface.ICHelloService m_service;

        public ICHelloService_HandlerMap(RpcTestInterface.ICHelloService service)
        {
            m_service = service;
            service.CallAsync.AddProtocolHandler((int)ProtoID.EICHelloService_Hello_MsgIn, Process_Hello);
            service.CallAsync.AddProtocolDeserializer((int)ProtoID.EICHelloService_Hello_MsgIn, Deserialize_Hello);
            service.CallAsync.AddProtocolHandler((int)ProtoID.EICHelloService_HelloInt_MsgIn, Process_HelloInt);
            service.CallAsync.AddProtocolDeserializer((int)ProtoID.EICHelloService_HelloInt_MsgIn, Deserialize_HelloInt);
            service.CallAsync.AddProtocolHandler((int)ProtoID.EICHelloService_Hello2_MsgIn, Process_Hello2);
            service.CallAsync.AddProtocolDeserializer((int)ProtoID.EICHelloService_Hello2_MsgIn, Deserialize_Hello2);
            service.CallAsync.AddProtocolHandler((int)ProtoID.EICHelloService_Hello3_MsgIn, Process_Hello3);
            service.CallAsync.AddProtocolDeserializer((int)ProtoID.EICHelloService_Hello3_MsgIn, Deserialize_Hello3);
        }

        private async MyTask Process_Hello(int iCommunicateID, IMessage _msg)
        {
            ICHelloService_Hello_MsgIn msg = (ICHelloService_Hello_MsgIn)_msg;
            m_service.Hello();
        }

        private IMessage Deserialize_Hello(byte[] bytes, int iStartIndex, int iCount)
        {
            ICHelloService_Hello_MsgIn msg = m_service.Serializer.Deserialize<ICHelloService_Hello_MsgIn>(bytes, iStartIndex, iCount);
            return msg;
        }

        private async MyTask Process_HelloInt(int iCommunicateID, IMessage _msg)
        {
            ICHelloService_HelloInt_MsgIn msg = (ICHelloService_HelloInt_MsgIn)_msg;
            var ret = await m_service.HelloInt(msg.a);
            ICHelloService_HelloInt_MsgOut msgRet = new ICHelloService_HelloInt_MsgOut();
            msgRet.Value = ret;
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = m_service.Serializer.Serialize(msgRet, buffer, start);
                return msgSerializeInfo;
            };
            m_service.CallAsync.SendWithoutResponse(m_service.ChunkType, iCommunicateID, (int)ProtoID.EICHelloService_HelloInt_MsgOut, f);
        }

        private IMessage Deserialize_HelloInt(byte[] bytes, int iStartIndex, int iCount)
        {
            ICHelloService_HelloInt_MsgIn msg = m_service.Serializer.Deserialize<ICHelloService_HelloInt_MsgIn>(bytes, iStartIndex, iCount);
            return msg;
        }

        private async MyTask Process_Hello2(int iCommunicateID, IMessage _msg)
        {
            ICHelloService_Hello2_MsgIn msg = (ICHelloService_Hello2_MsgIn)_msg;
            m_service.Hello2(msg.p);
        }

        private IMessage Deserialize_Hello2(byte[] bytes, int iStartIndex, int iCount)
        {
            ICHelloService_Hello2_MsgIn msg = m_service.Serializer.Deserialize<ICHelloService_Hello2_MsgIn>(bytes, iStartIndex, iCount);
            return msg;
        }

        private async MyTask Process_Hello3(int iCommunicateID, IMessage _msg)
        {
            ICHelloService_Hello3_MsgIn msg = (ICHelloService_Hello3_MsgIn)_msg;
            var ret = await m_service.Hello3(msg.p);
            ICHelloService_Hello3_MsgOut msgRet = new ICHelloService_Hello3_MsgOut();
            msgRet.Value = ret;
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = m_service.Serializer.Serialize(msgRet, buffer, start);
                return msgSerializeInfo;
            };
            m_service.CallAsync.SendWithoutResponse(m_service.ChunkType, iCommunicateID, (int)ProtoID.EICHelloService_Hello3_MsgOut, f);
        }

        private IMessage Deserialize_Hello3(byte[] bytes, int iStartIndex, int iCount)
        {
            ICHelloService_Hello3_MsgIn msg = m_service.Serializer.Deserialize<ICHelloService_Hello3_MsgIn>(bytes, iStartIndex, iCount);
            return msg;
        }
    }
}
