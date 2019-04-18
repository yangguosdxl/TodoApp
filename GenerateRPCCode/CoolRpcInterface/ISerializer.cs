using System;
using System.Collections.Generic;
using System.Text;

namespace CoolRpcInterface
{
    public interface ISerializer
    {
        (byte[], int, int) Serialize<T>(T o);
        T Deserialize<T>(byte[] bytes, int iStartIndex, int iCount);
    }
}
