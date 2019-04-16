using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsyncAwaitApp
{
    class MySynchronizationContext : SynchronizationContext
    {
        List<(SendOrPostCallback, object)> m_Events = new List<(SendOrPostCallback, object)>();
        public override void Send(SendOrPostCallback d, object state)
        {
            Post(d, state);
        }
        public override void Post(SendOrPostCallback d, object state)
        {
            //base.Post(d, state);
            m_Events.Add((d, state));
        }

        public void Update()
        {
            foreach(var e in m_Events)
            {
                e.Item1(e.Item2);
            }
            //m_Events.Clear();
        }
    }
}
