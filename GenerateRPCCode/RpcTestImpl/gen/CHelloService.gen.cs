using System;
using Cool.Interface.Rpc;
using System.Threading.Tasks;
using Cool.Coroutine;
using RpcTestInterface;

namespace CSRPC
{
    internal class CHelloService : RpcTestInterface.ICHelloService
    {
        public ICallAsync CallAsync { get; set; }

        public ISerializer Serializer { get; set; }

        public int ChunkType { get; set; }

        public void Init(ISerializer serializer, ICallAsync callAsync, int iChunkType)
        {
            Serializer = serializer;
            CallAsync = callAsync;
            ChunkType = iChunkType;
            CallAsync.AddProtocolDeserializer((int)ProtoID.EICHelloService_HelloInt_MsgOut, Serializer.Deserialize<ICHelloService_HelloInt_MsgOut>);
            CallAsync.AddProtocolDeserializer((int)ProtoID.EICHelloService_Hello3_MsgOut, Serializer.Deserialize<ICHelloService_Hello3_MsgOut>);
        }

        public CHelloService()
        {
        }

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
            var ret = (ICHelloService_HelloInt_MsgOut)await CallAsync.SendWithResponse(ChunkType, (int)ProtoID.EICHelloService_HelloInt_MsgIn, f);
            return ret.Value;
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
            var ret = (ICHelloService_Hello3_MsgOut)await CallAsync.SendWithResponse(ChunkType, (int)ProtoID.EICHelloService_Hello3_MsgIn, f);
            return ret.Value;
        }
    }
}
