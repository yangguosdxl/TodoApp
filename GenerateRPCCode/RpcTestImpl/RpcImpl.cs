using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CSRPC
{
    public enum ProtoID
    {
        EICHelloService_Hello_MsgIn,
        EICHelloService_HelloInt_MsgIn,
        EICHelloService_HelloInt_MsgOut,
        EICHelloService_Hello2_MsgIn,
        EICHelloService_Hello3_MsgIn,
        EICHelloService_Hello3_MsgOut,
        EISHelloService_Hello_MsgIn,
        EISHelloService_HelloInt_MsgIn,
        EISHelloService_HelloInt_MsgOut,
        EISHelloService_Hello2_MsgIn,
        EISHelloService_Hello3_MsgIn,
        EISHelloService_Hello3_MsgOut,
        COUNT
    }

    public static class RpcServicesConfiguration
    {
        public static void AddAllRpcServices(IServiceCollection ServiceCollection)
        {
            ServiceCollection.AddSingleton<RpcTestInterface.ICHelloService, CHelloService>();
            ServiceCollection.AddSingleton<RpcTestInterface.ISHelloService, SHelloService>();
        }
    }

    public class CHelloService : RpcTestInterface.ICHelloService
    {
        public ICallAsync CallAsync { get; set; }

        public ISerializer Serializer { get; set; }

        public async Task Hello()
        {
            ICHelloService_Hello_MsgIn msg = new ICHelloService_Hello_MsgIn();
            var msgSerializeInfo = Serializer.Serialize(msg);
            await CallAsync.SendWithoutResponse(0, (int)ProtoID.EICHelloService_Hello_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<ValueTuple<System.Int32, System.Int32>> HelloInt(System.Int32 a)
        {
            ICHelloService_HelloInt_MsgIn msg = new ICHelloService_HelloInt_MsgIn();
            msg.a = a;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(0, (int)ProtoID.EICHelloService_HelloInt_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<ICHelloService_HelloInt_MsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }

        public async Task Hello2(RpcTestInterface.Param p)
        {
            ICHelloService_Hello2_MsgIn msg = new ICHelloService_Hello2_MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            await CallAsync.SendWithoutResponse(0, (int)ProtoID.EICHelloService_Hello2_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<RpcTestInterface.Param> Hello3(RpcTestInterface.Param p)
        {
            ICHelloService_Hello3_MsgIn msg = new ICHelloService_Hello3_MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(0, (int)ProtoID.EICHelloService_Hello3_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<ICHelloService_Hello3_MsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }
    }

    public class ICHelloService_HandlerMap : DefaultRPCHandlerMap
    {
        private RpcTestInterface.ICHelloService m_service;

        public ICHelloService_HandlerMap(RpcTestInterface.ICHelloService service)
            : base((int)ProtoID.COUNT)
        {
            m_service = service;
            Add((int)ProtoID.EICHelloService_Hello_MsgIn, Process_Hello);
            Add((int)ProtoID.EICHelloService_HelloInt_MsgIn, Process_HelloInt);
            Add((int)ProtoID.EICHelloService_Hello2_MsgIn, Process_Hello2);
            Add((int)ProtoID.EICHelloService_Hello3_MsgIn, Process_Hello3);
        }

        private void Process_Hello(int iCommunicateID, byte[] bytes, int iStartIndex, int iCount)
        {
            ICHelloService_Hello_MsgIn msg = m_service.Serializer.Deserialize<ICHelloService_Hello_MsgIn>(bytes, iStartIndex, iCount);
            m_service.Hello();
        }

        private void Process_HelloInt(int iCommunicateID, byte[] bytes, int iStartIndex, int iCount)
        {
            ICHelloService_HelloInt_MsgIn msg = m_service.Serializer.Deserialize<ICHelloService_HelloInt_MsgIn>(bytes, iStartIndex, iCount);
            var v1 = m_service.HelloInt(msg.a);
            var v2 = v1.GetAwaiter();
            var ret = v2.GetResult();
            var ser = m_service.Serializer.Serialize(ret);
            m_service.CallAsync.SendWithoutResponse(iCommunicateID, (int)ProtoID.EICHelloService_HelloInt_MsgOut, ser.Item1, ser.Item2, ser.Item3);
        }

        private void Process_Hello2(int iCommunicateID, byte[] bytes, int iStartIndex, int iCount)
        {
            ICHelloService_Hello2_MsgIn msg = m_service.Serializer.Deserialize<ICHelloService_Hello2_MsgIn>(bytes, iStartIndex, iCount);
            m_service.Hello2(msg.p);
        }

        private void Process_Hello3(int iCommunicateID, byte[] bytes, int iStartIndex, int iCount)
        {
            ICHelloService_Hello3_MsgIn msg = m_service.Serializer.Deserialize<ICHelloService_Hello3_MsgIn>(bytes, iStartIndex, iCount);
            var v1 = m_service.Hello3(msg.p);
            var v2 = v1.GetAwaiter();
            var ret = v2.GetResult();
            var ser = m_service.Serializer.Serialize(ret);
            m_service.CallAsync.SendWithoutResponse(iCommunicateID, (int)ProtoID.EICHelloService_Hello3_MsgOut, ser.Item1, ser.Item2, ser.Item3);
        }
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_Hello_MsgIn
    {
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_HelloInt_MsgIn
    {
        [MessagePack.Key(1)]
        public System.Int32 a;
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_HelloInt_MsgOut
    {
        [MessagePack.Key(1)]
        public ValueTuple<System.Int32, System.Int32> Value;
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_Hello2_MsgIn
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param p;
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_Hello3_MsgIn
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param p;
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_Hello3_MsgOut
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param Value;
    }

    public class SHelloService : RpcTestInterface.ISHelloService
    {
        public ICallAsync CallAsync { get; set; }

        public ISerializer Serializer { get; set; }

        public async Task Hello()
        {
            ISHelloService_Hello_MsgIn msg = new ISHelloService_Hello_MsgIn();
            var msgSerializeInfo = Serializer.Serialize(msg);
            await CallAsync.SendWithoutResponse(0, (int)ProtoID.EISHelloService_Hello_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<ValueTuple<System.Int32, System.Int32>> HelloInt(System.Int32 a)
        {
            ISHelloService_HelloInt_MsgIn msg = new ISHelloService_HelloInt_MsgIn();
            msg.a = a;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(0, (int)ProtoID.EISHelloService_HelloInt_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<ISHelloService_HelloInt_MsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }

        public async Task Hello2(RpcTestInterface.Param p)
        {
            ISHelloService_Hello2_MsgIn msg = new ISHelloService_Hello2_MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            await CallAsync.SendWithoutResponse(0, (int)ProtoID.EISHelloService_Hello2_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<RpcTestInterface.Param> Hello3(RpcTestInterface.Param p)
        {
            ISHelloService_Hello3_MsgIn msg = new ISHelloService_Hello3_MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(0, (int)ProtoID.EISHelloService_Hello3_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<ISHelloService_Hello3_MsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }
    }

    public class ISHelloService_HandlerMap : DefaultRPCHandlerMap
    {
        private RpcTestInterface.ISHelloService m_service;

        public ISHelloService_HandlerMap(RpcTestInterface.ISHelloService service)
            : base((int)ProtoID.COUNT)
        {
            m_service = service;
            Add((int)ProtoID.EISHelloService_Hello_MsgIn, Process_Hello);
            Add((int)ProtoID.EISHelloService_HelloInt_MsgIn, Process_HelloInt);
            Add((int)ProtoID.EISHelloService_Hello2_MsgIn, Process_Hello2);
            Add((int)ProtoID.EISHelloService_Hello3_MsgIn, Process_Hello3);
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
            m_service.CallAsync.SendWithoutResponse(iCommunicateID, (int)ProtoID.EISHelloService_HelloInt_MsgOut, ser.Item1, ser.Item2, ser.Item3);
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
            m_service.CallAsync.SendWithoutResponse(iCommunicateID, (int)ProtoID.EISHelloService_Hello3_MsgOut, ser.Item1, ser.Item2, ser.Item3);
        }
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_Hello_MsgIn
    {
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_HelloInt_MsgIn
    {
        [MessagePack.Key(1)]
        public System.Int32 a;
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_HelloInt_MsgOut
    {
        [MessagePack.Key(1)]
        public ValueTuple<System.Int32, System.Int32> Value;
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_Hello2_MsgIn
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param p;
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_Hello3_MsgIn
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param p;
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_Hello3_MsgOut
    {
        [MessagePack.Key(1)]
        public RpcTestInterface.Param Value;
    }
}
