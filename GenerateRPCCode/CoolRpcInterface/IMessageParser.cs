using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public interface IMessageParser
    {
        // protocol id, response index, buffer, buffer start, buffer len
        event Action<int, int, byte[], int, int> OnMessage;

        void Process(byte[] buff, int start, int len);
    }
}
