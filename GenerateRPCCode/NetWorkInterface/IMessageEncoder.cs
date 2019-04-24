using System;

namespace NetWorkInterface
{
    public interface IMessageEncoder
    {
        (byte[] buff, int start, int len) Encode(int iChunkType, int iCommunicateID, int iProtoID, byte[] bytes, int iStart, int len);
    }
}
