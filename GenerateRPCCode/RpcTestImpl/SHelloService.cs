using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CSRPC
{
    public class SHelloService : RpcTestInterface.ISHelloService
    {
        public ICallAsync CallAsync { get; set; }

        public ISerializer Serializer { get; set; }

        public int ChunkType { get; set; }

        public async Task Hello()
        {
            ISHelloService_Hello_MsgIn msg = new ISHelloService_Hello_MsgIn();
            var msgSerializeInfo = Serializer.Serialize(msg);
            await CallAsync.SendWithoutResponse(ChunkType, 0, (int)ProtoID.EISHelloService_Hello_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<ValueTuple<System.Int32, System.Int32>> HelloInt(System.Int32 a)
        {
            ISHelloService_HelloInt_MsgIn msg = new ISHelloService_HelloInt_MsgIn();
            msg.a = a;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(ChunkType, (int)ProtoID.EISHelloService_HelloInt_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<ISHelloService_HelloInt_MsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }

        public async Task Hello2(RpcTestInterface.Param p)
        {
            ISHelloService_Hello2_MsgIn msg = new ISHelloService_Hello2_MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            await CallAsync.SendWithoutResponse(ChunkType, 0, (int)ProtoID.EISHelloService_Hello2_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
        }

        public async Task<RpcTestInterface.Param> Hello3(RpcTestInterface.Param p)
        {
            ISHelloService_Hello3_MsgIn msg = new ISHelloService_Hello3_MsgIn();
            msg.p = p;
            var msgSerializeInfo = Serializer.Serialize(msg);
            var ret = await CallAsync.SendWithResponse(ChunkType, (int)ProtoID.EISHelloService_Hello3_MsgIn, msgSerializeInfo.Item1, msgSerializeInfo.Item2, msgSerializeInfo.Item3);
            var retMsg = Serializer.Deserialize<ISHelloService_Hello3_MsgOut>(ret.Item1, ret.Item2, ret.Item3);
            return await Task.FromResult(retMsg.Value);
        }
    }
}
