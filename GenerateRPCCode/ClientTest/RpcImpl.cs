using System;
using CoolRpcInterface;
using System.Threading.Tasks;

namespace CSRPC
{
    public enum ProtoID : byte
    {
        EHelloMsgIn,
        EHelloIntMsgIn,
        EHelloIntMsgOut
    }

    public class HelloService : RpcTestInterface.IHelloService
    {
        public ICallAsync CallAsync { get; set; }

        public ISerializer Serializer { get; set; }

        public HelloService(ICallAsync callAsync, ISerializer serializer)
        {
            CallAsync = callAsync;
            Serializer = serializer;
        }

        public async Task Hello()
        {
            HelloMsgIn msg = new HelloMsgIn();
            var msgSerializeInfo = Serializer.Serialize(msg);
            await CallAsync.SendWithoutResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<ValueTuple<System.Int32, System.Int32>> HelloInt(Int32 a)
        {
            HelloIntMsgIn msg = new HelloIntMsgIn();
            msg.a = a;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<HelloIntMsgOut>(ret.Item1, ret.Item2, ret.Item3);
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
}
