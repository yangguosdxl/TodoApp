using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterface
{
    public interface IPlayerGrain : IClientSessionGrain, IGrainWithGuidKey
    {
        Task Hello();
    }
}
