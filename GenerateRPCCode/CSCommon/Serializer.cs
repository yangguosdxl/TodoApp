using CoolRpcInterface;
using System;
using System.IO;

namespace CSCommon
{
    public class Serializer : ISerializer
    {
        public T Deserialize<T>(byte[] bytes, int iStartIndex, int iCount)
        {
            ArraySegment<byte> seg = new ArraySegment<byte>(bytes, iStartIndex, iCount);
            return MessagePack.LZ4MessagePackSerializer.Deserialize<T>(seg);
        }

        public (byte[], int, int) Serialize<T>(T o, byte[] buffer, int start)
        {
            int len = MessagePack.LZ4MessagePackSerializer.SerializeToBlock(ref buffer, start, o, MessagePack.Resolvers.StandardResolver.Instance);
            return (buffer, start, len);
        }

        public (byte[], int, int) Serialize<T>(T o)
        {
            throw new NotImplementedException();
        }
    }
}
