
using System.Threading.Tasks;

namespace NetWorkInterface
{
    public interface ISocketAcceptor
    {
        Task<ISocket> AcceptAsync();
    }
}
