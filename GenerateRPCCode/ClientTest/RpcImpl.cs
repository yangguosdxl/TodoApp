using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CSRPC
{
    public enum ProtoID
    {
        EHelloMsgIn,
        EHelloIntMsgIn,
        EHelloIntMsgOut,
        EHello2MsgIn,
        EHello3MsgIn,
        EHello3MsgOut
    }

    public static class RpcServicesConfiguration
    {
        public static void AddAllRpcServices(IServiceCollection ServiceCollection)
        {
            ServiceCollection.AddSingleton<RpcTestInterface.IHelloService, HelloService>();
        }
    }

    public class HelloService : RpcTestInterface.IHelloService
    {
        public ICallAsync CallAsync { get; set; }

        public ISerializer Serializer { get; set; }

        public async Task Hello()
        {
            HelloMsgIn msg = new HelloMsgIn();
            var msgSerializeInfo = Serializer.Serialize(msg);
            await CallAsync.SendWithoutResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<ValueTuple<System.Int32, System.Int32>> HelloInt(System.Int32 a)
        {
            HelloIntMsgIn msg = new HelloIntMsgIn();
            msg.a = a;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<HelloIntMsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }

        public async Task Hello2(RpcTestInterface.Param p)
        {
            Hello2MsgIn msg = new Hello2MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            await CallAsync.SendWithoutResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<RpcTestInterface.Param> Hello3(RpcTestInterface.Param p)
        {
            Hello3MsgIn msg = new Hello3MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<Hello3MsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }
    }

    [MessagePack.MessagePackObject]
    public class HelloMsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EHelloMsgIn;
    }

    [MessagePack.MessagePackObject]
    public class HelloIntMsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EHelloIntMsgIn;
        [MessagePack.Key(2)]
        public System.Int32 a;
    }

    [MessagePack.MessagePackObject]
    public class HelloIntMsgOut
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EHelloIntMsgOut;
        [MessagePack.Key(2)]
        public ValueTuple<System.Int32, System.Int32> Value;
    }

    [MessagePack.MessagePackObject]
    public class Hello2MsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EHello2MsgIn;
        [MessagePack.Key(2)]
        public RpcTestInterface.Param p;
    }

    [MessagePack.MessagePackObject]
    public class Hello3MsgIn
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EHello3MsgIn;
        [MessagePack.Key(2)]
        public RpcTestInterface.Param p;
    }

    [MessagePack.MessagePackObject]
    public class Hello3MsgOut
    {
        [MessagePack.Key(1)]
        public ProtoID eProtoID = ProtoID.EHello3MsgOut;
        [MessagePack.Key(2)]
        public RpcTestInterface.Param Value;
    }
}
