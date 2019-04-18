using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public interface IMessageParser
    {
        void Process(byte[] buff, int start, int len);
    }
}
