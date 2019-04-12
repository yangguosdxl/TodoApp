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
        EISHelloService_Hello3_MsgOut
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
            await CallAsync.SendWithoutResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<ValueTuple<System.Int32, System.Int32>> HelloInt(System.Int32 a)
        {
            ICHelloService_HelloInt_MsgIn msg = new ICHelloService_HelloInt_MsgIn();
            msg.a = a;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<ICHelloService_HelloInt_MsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }

        public async Task Hello2(RpcTestInterface.Param p)
        {
            ICHelloService_Hello2_MsgIn msg = new ICHelloService_Hello2_MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            await CallAsync.SendWithoutResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<RpcTestInterface.Param> Hello3(RpcTestInterface.Param p)
        {
            ICHelloService_Hello3_MsgIn msg = new ICHelloService_Hello3_MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<ICHelloService_Hello3_MsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_Hello_MsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EICHelloService_Hello_MsgIn;
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_HelloInt_MsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EICHelloService_HelloInt_MsgIn;
        [MessagePack.Key(2)]
        public System.Int32 a;
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_HelloInt_MsgOut
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EICHelloService_HelloInt_MsgOut;
        [MessagePack.Key(2)]
        public ValueTuple<System.Int32, System.Int32> Value;
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_Hello2_MsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EICHelloService_Hello2_MsgIn;
        [MessagePack.Key(2)]
        public RpcTestInterface.Param p;
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_Hello3_MsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EICHelloService_Hello3_MsgIn;
        [MessagePack.Key(2)]
        public RpcTestInterface.Param p;
    }

    [MessagePack.MessagePackObject]
    public class ICHelloService_Hello3_MsgOut
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EICHelloService_Hello3_MsgOut;
        [MessagePack.Key(2)]
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
            await CallAsync.SendWithoutResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<ValueTuple<System.Int32, System.Int32>> HelloInt(System.Int32 a)
        {
            ISHelloService_HelloInt_MsgIn msg = new ISHelloService_HelloInt_MsgIn();
            msg.a = a;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<ISHelloService_HelloInt_MsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }

        public async Task Hello2(RpcTestInterface.Param p)
        {
            ISHelloService_Hello2_MsgIn msg = new ISHelloService_Hello2_MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            await CallAsync.SendWithoutResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<RpcTestInterface.Param> Hello3(RpcTestInterface.Param p)
        {
            ISHelloService_Hello3_MsgIn msg = new ISHelloService_Hello3_MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<ISHelloService_Hello3_MsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_Hello_MsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EISHelloService_Hello_MsgIn;
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_HelloInt_MsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EISHelloService_HelloInt_MsgIn;
        [MessagePack.Key(2)]
        public System.Int32 a;
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_HelloInt_MsgOut
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EISHelloService_HelloInt_MsgOut;
        [MessagePack.Key(2)]
        public ValueTuple<System.Int32, System.Int32> Value;
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_Hello2_MsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EISHelloService_Hello2_MsgIn;
        [MessagePack.Key(2)]
        public RpcTestInterface.Param p;
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_Hello3_MsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EISHelloService_Hello3_MsgIn;
        [MessagePack.Key(2)]
        public RpcTestInterface.Param p;
    }

    [MessagePack.MessagePackObject]
    public class ISHelloService_Hello3_MsgOut
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EISHelloService_Hello3_MsgOut;
        [MessagePack.Key(2)]
        public RpcTestInterface.Param Value;
    }
}
