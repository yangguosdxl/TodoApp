using Cool;
using GrainInterface;
using Orleans;
using RpcTestInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainsTest
{
    public class PlayerGrain : ClientSessionGrain, IPlayerGrain
    {
        public override Task OnActivateAsync()
        {
            Task task = base.OnActivateAsync();

            CallAsync.AddRpcHandlers<ISHelloService>(new SHelloService(this));

            return task;
        }
        public Task Hello()
        {
            Logger.Info("Player grain HELLOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");

            return Task.CompletedTask;
        }
    }
}
