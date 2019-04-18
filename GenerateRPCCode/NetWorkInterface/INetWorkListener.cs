using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetWorkInterface
{
    public interface INetWorkListener
    {
        Task<IChannel> Accept();
    }
}
