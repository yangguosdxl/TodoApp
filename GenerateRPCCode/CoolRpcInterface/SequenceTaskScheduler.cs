using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoolRpcInterface
{
    public class SequenceTaskScheduler : TaskScheduler
    {
        ConcurrentQueue<Task> m_Tasks = new ConcurrentQueue<Task>();

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return m_Tasks;
        }

        protected override void QueueTask(Task task)
        {
            m_Tasks.Enqueue(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {

        }
    }
}
