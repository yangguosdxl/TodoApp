using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public interface IProtocolUnit
    {
        (byte[], int start, int len) Process(byte[] buff, int start, int len);
    }
}
