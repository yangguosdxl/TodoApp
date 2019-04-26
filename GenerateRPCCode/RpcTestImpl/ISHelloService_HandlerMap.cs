using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CSRPC
{
    public class ISHelloService_HandlerMap : IRPCHandlerMap
    {
        private RpcTestInterface.ISHelloService m_service;

        public ISHelloService_HandlerMap(RpcTestInterface.ISHelloService service)
        {
            m_service = service;
            service.CallAsync.AddProtocolHandler((int)ProtoID.EISHelloService_Hello_MsgIn, Process_Hello);
            service.CallAsync.AddProtocolHandler((int)ProtoID.EISHelloService_HelloInt_MsgIn, Process_HelloInt);
            service.CallAsync.AddProtocolHandler((int)ProtoID.EISHelloService_Hello2_MsgIn, Process_Hello2);
            service.CallAsync.AddProtocolHandler((int)ProtoID.EISHelloService_Hello3_MsgIn, Process_Hello3);
        }

        private void Process_Hello(int iCommunicateID, byte[] bytes, int iStartIndex, int iCount)
        {
            ISHelloService_Hello_MsgIn msg = m_service.Serializer.Deserialize<ISHelloService_Hello_MsgIn>(bytes, iStartIndex, iCount);
            m_service.Hello();
        }

        private void Process_HelloInt(int iCommunicateID, byte[] bytes, int iStartIndex, int iCount)
        {
            ISHelloService_HelloInt_MsgIn msg = m_service.Serializer.Deserialize<ISHelloService_HelloInt_MsgIn>(bytes, iStartIndex, iCount);
            var v1 = m_service.HelloInt(msg.a);
            var v2 = v1.GetAwaiter();
            var ret = v2.GetResult();
            var ser = m_service.Serializer.Serialize(ret);
            m_service.CallAsync.SendWithoutResponse(m_service.ChunkType, iCommunicateID, (int)ProtoID.EISHelloService_HelloInt_MsgOut, ser.Item1, ser.Item2, ser.Item3);
        }

        private void Process_Hello2(int iCommunicateID, byte[] bytes, int iStartIndex, int iCount)
        {
            ISHelloService_Hello2_MsgIn msg = m_service.Serializer.Deserialize<ISHelloService_Hello2_MsgIn>(bytes, iStartIndex, iCount);
            m_service.Hello2(msg.p);
        }

        private void Process_Hello3(int iCommunicateID, byte[] bytes, int iStartIndex, int iCount)
        {
            ISHelloService_Hello3_MsgIn msg = m_service.Serializer.Deserialize<ISHelloService_Hello3_MsgIn>(bytes, iStartIndex, iCount);
            var v1 = m_service.Hello3(msg.p);
            var v2 = v1.GetAwaiter();
            var ret = v2.GetResult();
            var ser = m_service.Serializer.Serialize(ret);
            m_service.CallAsync.SendWithoutResponse(m_service.ChunkType, iCommunicateID, (int)ProtoID.EISHelloService_Hello3_MsgOut, ser.Item1, ser.Item2, ser.Item3);
        }
    }
}
