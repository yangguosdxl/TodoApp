using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsyncAwaitApp.Coroutine
{
    class MyTaskScheduler
    {
        List<MyTask> m_Tasks = new List<MyTask>();
        List<MyTask> m_NewTasks = new List<MyTask>();

        public void QueueTask(MyTask task)
        {
            m_NewTasks.Add(task);
        }

        public void Update()
        {
            m_Tasks.AddRange(m_NewTasks);
            m_NewTasks.Clear();
            List<int> aCompleteTasks = new List<int>();
            for(int i = 0; i < m_Tasks.Count; ++i)
            {
                MyTask t = m_Tasks[i];
                try
                {
                    t.Action();
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
