using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsyncAwaitApp
{
    class CustomTaskScheduler : TaskScheduler
    {
        List<Task> m_Tasks = new List<Task>();
        List<Task> m_NewTasks = new List<Task>();

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return m_Tasks;
        }

        protected override void QueueTask(Task task)
        {
            m_NewTasks.Add(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        public void Update()
        {
            m_Tasks.AddRange(m_NewTasks);
            m_NewTasks.Clear();
            List<int> aCompleteTasks = new List<int>();
            for(int i = 0; i < m_Tasks.Count; ++i)
            {
                Task t = m_Tasks[i];
                try
                {
                    base.TryExecuteTask(t);
                    if (t.IsCompleted)
                        aCompleteTasks.Add(i);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"exception {e}");
                }
            }
            foreach (int i in aCompleteTasks)
                m_Tasks.RemoveAt(i);
        }
    }
}
