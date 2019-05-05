using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;

namespace CSRPC
{
    public class SHelloService : RpcTestInterface.ISHelloService
    {
        public ICallAsync CallAsync { get; set; }

        public ISerializer Serializer { get; set; }

        public int ChunkType { get; set; }

        public void Hello()
        {
            ISHelloService_Hello_MsgIn msg = new ISHelloService_Hello_MsgIn();
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = Serializer.Serialize(msg, buffer, start);
                return msgSerializeInfo;
            };
            CallAsync.SendWithoutResponse(ChunkType, 0, (int)ProtoID.EISHelloService_Hello_MsgIn, f);
        }

        public async MyTask<ValueTuple<System.Int32, System.Int32>> HelloInt(System.Int32 a)
        {
            ISHelloService_HelloInt_MsgIn msg = new ISHelloService_HelloInt_MsgIn();
            msg.a = a;
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = Serializer.Serialize(msg, buffer, start);
                return msgSerializeInfo;
            };
            var ret = await CallAsync.SendWithResponse<ISHelloService_HelloInt_MsgOut>(ChunkType, (int)ProtoID.EISHelloService_HelloInt_MsgIn, f);
            return ret.Value;
        }

        public void Hello2(RpcTestInterface.Param p)
        {
            ISHelloService_Hello2_MsgIn msg = new ISHelloService_Hello2_MsgIn();
            msg.p = p;
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = Serializer.Serialize(msg, buffer, start);
                return msgSerializeInfo;
            };
            CallAsync.SendWithoutResponse(ChunkType, 0, (int)ProtoID.EISHelloService_Hello2_MsgIn, f);
        }

        public async MyTask<RpcTestInterface.Param> Hello3(RpcTestInterface.Param p)
        {
            ISHelloService_Hello3_MsgIn msg = new ISHelloService_Hello3_MsgIn();
            msg.p = p;
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = Serializer.Serialize(msg, buffer, start);
                return msgSerializeInfo;
            };
            var ret = await CallAsync.SendWithResponse<ISHelloService_Hello3_MsgOut>(ChunkType, (int)ProtoID.EISHelloService_Hello3_MsgIn, f);
            return ret.Value;
        }
    }
}
