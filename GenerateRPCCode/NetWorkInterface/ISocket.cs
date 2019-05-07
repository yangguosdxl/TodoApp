using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetWorkInterface
{
    public interface ISocket
    {
        Task<int> SendAsync(ArraySegment<byte> seg);
        Task<int> RecvAsync(ArraySegment<byte> seg);

        void Dispose();
    }
}
