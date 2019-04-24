using System;

namespace NetWorkInterface
{
    public interface IMessageDecoder
    {
        // protocol id, iCommunicateID, buffer, buffer start, buffer len
        // after this message be called, buffer will be change
        event Action<int, int, byte[], int, int> OnMessage;

        void Decode(byte[] buff, int start, int len);
    }
}
