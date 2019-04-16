using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestAsyncAwaitApp
{
    class MyTaskScheduler : TaskScheduler
    {
        List<Task> m_Tasks = new List<Task>();
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return m_Tasks;
        }

        protected override void QueueTask(Task task)
        {
            m_Tasks.Add(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        public void Update()
        {
            foreach(Task t in m_Tasks)
            {
                try
                {
                    base.TryExecuteTask(t);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"exception {e}");
                }
                
            }
        }
    }
}
