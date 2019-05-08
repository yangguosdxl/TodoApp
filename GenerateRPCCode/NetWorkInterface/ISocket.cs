using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetWorkInterface
{
    public interface ISocket
    {
        // 线程安全
        Task<int> SendAsync(ArraySegment<byte> seg);

        // 线程安全
        Task<int> RecvAsync(ArraySegment<byte> seg);

        void Dispose();
    }
}
