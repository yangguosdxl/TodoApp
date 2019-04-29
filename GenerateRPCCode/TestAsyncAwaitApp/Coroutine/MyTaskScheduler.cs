using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsyncAwaitApp.Coroutine
{
    public class MyTaskScheduler
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
                    if (t.Status == MyTaskStatus.Active)
                    {
                        if (t.Action != null)
                            t.Action();
                    }
                    
                    if (t.Status == MyTaskStatus.Complete)
                    {
                        t.OnCompleted();
                        aCompleteTasks.Add(i);
                    }
                    else if (t.Status == MyTaskStatus.Exception)
                    {
                        aCompleteTasks.Add(i);

                        foreach (Exception e in t.GetException().InnerExceptions)
                            Console.WriteLine(e);
                    }
                    else if (t.Status == MyTaskStatus.Canceled)
                    {
                        aCompleteTasks.Add(i);
                    }

                        
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
