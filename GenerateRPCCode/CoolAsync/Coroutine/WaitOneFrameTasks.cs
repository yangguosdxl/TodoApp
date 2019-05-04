
using System.Collections.Generic;

namespace Cool.Coroutine
{
    public class WaitOneFrameTasks
    {
        private List<MyTask> m_Tasks = new List<MyTask>();

        private void Add(MyTask task)
        {
            m_Tasks.Add(task);
        }

        public MyTask WaitOneFrame()
        {
            MyTask task = new MyTask();
            task.szName = "WaitOneFrameTask";

            Add(task);

            return task;
        }

        public void Update()
        {
            for(int i = m_Tasks.Count-1; i >= 0; --i)
            {
                MyTask task = m_Tasks[i];
                m_Tasks.RemoveAt(i);

                if (task.Status != MyTaskStatus.Canceled)
                    task.SetResult();
            }
        }
    }
}
