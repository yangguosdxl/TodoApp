using System;

namespace NetWorkInterface
{
    public interface IMessageCoder
    {
        // protocol id, iCommunicateID, buffer, buffer start, buffer len
        // after this message be called, buffer will be change
        event Action<int, int, byte[], int, int> OnMessage;

        void Decode(byte[] buff, int start, int len);

        (byte[] buff, int start, int len) Encode(int iCommunicateID, int iProtoID, byte[] bytes, int iStart, int len);
    }
}
