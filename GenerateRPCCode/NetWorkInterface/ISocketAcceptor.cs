
using System.Threading.Tasks;

namespace NetWorkInterface
{
    public interface ISocketAcceptor
    {
        // 线程安全
        Task<ISocket> AcceptAsync();
    }
}
