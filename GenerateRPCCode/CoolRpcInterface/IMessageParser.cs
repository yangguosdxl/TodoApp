using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public interface IMessageParser
    {
        // protocol id, iCommunicateID, buffer, buffer start, buffer len
        // after this message be called, buffer will be change
        event Action<int, int, byte[], int, int> OnMessage;

        void Process(byte[] buff, int start, int len);
    }
}
