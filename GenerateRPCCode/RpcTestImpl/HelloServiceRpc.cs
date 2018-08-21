using CoolRpcInterface;
using RpcTestInterface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RpcTestImpl
{
    public enum MsgID
    {
        Hello,
        HelloInt,
        HelloIntRet,
    }

    [MessagePack.MessagePackObject]
    public class MsgHello
    {
        [MessagePack.Key(1)]
        public MsgID id = MsgID.Hello;

    }

    [MessagePack.MessagePackObject]
    public class MsgHelloInt
    {
        [MessagePack.Key(1)]
        public MsgID id = MsgID.HelloInt;

        [MessagePack.Key(2)]
        public int a;
    }

    [MessagePack.MessagePackObject]
    public class MsgHelloIntRet
    {
        [MessagePack.Key(1)]
        public MsgID id = MsgID.HelloIntRet;

        [MessagePack.Key(2)]
        public int a;
    }


    public class HelloServiceRpc : IHelloService
    {
        private ICallAsync m_CallAsync;
        private ISerializer m_Serializer;
        public HelloServiceRpc(ICallAsync callAsync, ISerializer serializer)
        {
            m_CallAsync = callAsync;
        }
        public async Task Hello()
        {
            var msg = new MsgHello();

            (byte[] bytes, int iStart, int len) = m_Serializer.Serialize(msg);

            await m_CallAsync.Send(bytes, iStart, len);
        }

        public async Task<(int, int)> HelloInt(int a)
        {
            var msg = new MsgHelloInt();

            //var (bytes, iStart, len ) = m_Serializer.Serialize(msg);
            var sz = m_Serializer.Serialize(msg);
            sz.

            var (byteRet, indexRet, lenRet) = await m_CallAsync.SendWithResponse(bytes, iStart, len);

            var ret = m_Serializer.Deserialize<MsgHelloIntRet>(byteRet, indexRet, lenRet);
            
            return await Task.FromResult((ret.a, ret.a));
        }
    }
}
