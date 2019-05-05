using System;
using CoolRpcInterface;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Cool.Coroutine;

namespace CSRPC
{
    public class CHelloService : RpcTestInterface.ICHelloService
    {
        public ICallAsync CallAsync { get; set; }

        public ISerializer Serializer { get; set; }

        public int ChunkType { get; set; }

        public void Hello()
        {
            ICHelloService_Hello_MsgIn msg = new ICHelloService_Hello_MsgIn();
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = Serializer.Serialize(msg, buffer, start);
                return msgSerializeInfo;
            };
            CallAsync.SendWithoutResponse(ChunkType, 0, (int)ProtoID.EICHelloService_Hello_MsgIn, f);
        }

        public async MyTask<ValueTuple<System.Int32, System.Int32>> HelloInt(System.Int32 a)
        {
            ICHelloService_HelloInt_MsgIn msg = new ICHelloService_HelloInt_MsgIn();
            msg.a = a;
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = Serializer.Serialize(msg, buffer, start);
                return msgSerializeInfo;
            };
            var ret = await CallAsync.SendWithResponse<ICHelloService_HelloInt_MsgOut>(ChunkType, (int)ProtoID.EICHelloService_HelloInt_MsgIn, f);
            return ret.Value;
            msgRet.Value = ret;
        }

        public void Hello2(RpcTestInterface.Param p)
        {
            ICHelloService_Hello2_MsgIn msg = new ICHelloService_Hello2_MsgIn();
            msg.p = p;
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = Serializer.Serialize(msg, buffer, start);
                return msgSerializeInfo;
            };
            CallAsync.SendWithoutResponse(ChunkType, 0, (int)ProtoID.EICHelloService_Hello2_MsgIn, f);
        }

        public async MyTask<RpcTestInterface.Param> Hello3(RpcTestInterface.Param p)
        {
            ICHelloService_Hello3_MsgIn msg = new ICHelloService_Hello3_MsgIn();
            msg.p = p;
            Func<byte[], int, ValueTuple<byte[], int, int>> f = delegate(byte[] buffer, int start)
            {
                var msgSerializeInfo = Serializer.Serialize(msg, buffer, start);
                return msgSerializeInfo;
            };
            var ret = await CallAsync.SendWithResponse<ICHelloService_Hello3_MsgOut>(ChunkType, (int)ProtoID.EICHelloService_Hello3_MsgIn, f);
            return ret.Value;
            msgRet.Value = ret;
        }
    }
}
