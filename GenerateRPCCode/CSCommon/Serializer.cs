using CoolRpcInterface;
using System;
using System.IO;

namespace CSCommon
{
    public class Serializer : ISerializer
    {
        public T Deserialize<T>(byte[] bytes, int iStartIndex, int iCount)
        {

            MemoryStream msRet = new MemoryStream(bytes, iStartIndex, iCount, false);

            return MessagePack.MessagePackSerializer.Deserialize<T>(msRet);
        }

        public (byte[], int, int) Serialize<T>(T o)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                MessagePack.MessagePackSerializer.Serialize(ms, o);
                return (ms.GetBuffer(), 0, (int)ms.Position);
            }
        }
    }
}
