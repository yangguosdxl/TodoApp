using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoolRpcInterface
{
    public interface ICallAsync
    {
        Task Send(byte[] bytes, int iStart, int len);
        Task<(byte[], int, int)> SendWithResponse(byte[] bytes, int iStart, int len);
    }
}
