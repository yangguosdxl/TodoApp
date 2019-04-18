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
        List<(SendOrPostCallback, object)> m_NewEvents = new List<(SendOrPostCallback, object)>();

        bool m_bRemoveEvent = false;

        public override void Send(SendOrPostCallback d, object state)
        {
            Post(d, state);
        }
        public override void Post(SendOrPostCallback d, object state)
        {
            //base.Post(d, state);
            m_NewEvents.Add((d, state));
        }

        public override void OperationCompleted()
        {
            m_bRemoveEvent = true;
        }

        public void Update()
        {
            List<int> aNeedRemoved = new List<int>();
            for(int i = m_Events.Count - 1; i >= 0; --i)
            {
                var e = m_Events[i];
                e.Item1(e.Item2);

                if (m_bRemoveEvent)
                {
                    m_Events.RemoveAt(i); 
                }
            }

            m_Events.AddRange(m_NewEvents);
            m_NewEvents.Clear();
            //m_Events.Clear();
        }
    }
}
