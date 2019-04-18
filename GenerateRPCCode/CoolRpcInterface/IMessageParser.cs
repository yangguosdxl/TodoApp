using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public interface IMessageParser
    {
        void Process(ArraySegment<byte> seg);
    }
}
