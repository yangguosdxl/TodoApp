using Cool;
using GrainInterface;
using Orleans;
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

            return base.OnActivateAsync();
        }
        public Task Hello()
        {
            Logger.Info("Player grain HELLOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");

            return Task.CompletedTask;
        }
    }
}
