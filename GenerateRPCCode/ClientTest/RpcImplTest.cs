using Cool.Coroutine;
using CoolRpcInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientTest.RpcImpl
{
    public interface ICHelloService : ICoolRpc
    {
        void Hello();

        MyTask<(int, int)> HelloInt(int a);
    }

    public interface ISHelloService : ICoolRpc
    {
        void Hello();

        MyTask<(int, int)> HelloInt(int a);
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
    public class ICHelloService_HelloInt_MsgOut : IMessage
    {
        [MessagePack.Key(1)]
        public ValueTuple<System.Int32, System.Int32> Value;
    }

    public class CHelloServiceProxy : ICHelloService
    {
        public ISerializer Serializer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICallAsync CallAsync { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ChunkType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Hello()
        {
            Func<byte[], int, (byte[], int,int)> f = delegate(byte[] buffer, int start)
            {
                var data = Serializer.Serialize(new ICHelloService_Hello_MsgIn());

                return data;
            };

            CallAsync.SendWithoutResponse(f);
        }

        

        public async MyTask<(int, int)> HelloInt(int a)
        {
            Func<(byte[], int, int)> f = delegate
            {
                var data = Serializer.Serialize(new ICHelloService_HelloInt_MsgIn());

                return data;
            };

            ICHelloService_HelloInt_MsgOut msg = await CallAsync.SendWithResponse<ICHelloService_HelloInt_MsgOut>(f);

            return msg.Value;
        }
    }
}
