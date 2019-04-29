using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsyncAwaitApp.Coroutine
{
    public class MyTaskScheduler
    {
        public static MyTaskScheduler Current;

        List<MyTask> m_WaitTasks = new List<MyTask>();
        List<MyTask> m_Tasks = new List<MyTask>();

        public void QueueTask(MyTask task)
        {
            if (task.Status == MyTaskStatus.Wait)
                m_WaitTasks.Add(task);
            else
                m_Tasks.Add(task);
        }

        public void Update()
        {
            List<int> aCompleteTasks = new List<int>();
            for(int i = m_Tasks.Count  -1; i >= 0; --i)
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


                        MyTask.Current = t.Parent;

                        t.OnCompleted?.Invoke();
                        aCompleteTasks.Add(i);

                        if (t.Parent != null && t.Parent.Status != MyTaskStatus.Wait)
                        {
                            m_WaitTasks.Remove(t.Parent);
                            m_Tasks.Add(t.Parent);
                        }
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
