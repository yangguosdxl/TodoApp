using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace TestAsyncAwaitApp.Coroutine
{
    public interface IMyAwaiter : INotifyCompletion
    {
        MyTask Task { get; }
    }
    public class MyAwaiter : IMyAwaiter
    {
        protected MyTask m_Task;

        public bool IsCompleted => m_Task.IsCompleted;

        public MyTask Task => m_Task;

        public void GetResult()
        {
        }

        public MyAwaiter(MyTask task)
        {
            m_Task = task;
        }

        public void OnCompleted(Action continuation)
        {
            m_Task.OnCompleted = continuation;

            if (m_Task.Parent != null)
                m_Task.Parent.Status = MyTaskStatus.Wait;

            m_Task.scheduler.QueueTask(m_Task);
        }
    }

    public class WaitOneFrameAwaiter : IMyAwaiter
    {
        protected MyTask m_Task;

        public bool IsCompleted => m_Task.IsCompleted;

        public MyTask Task => m_Task;

        public void GetResult()
        {
        }

        public WaitOneFrameAwaiter(MyTask task)
        {
            m_Task = task;
        }

        public virtual void OnCompleted(Action continuation)
        {
            m_Task.OnCompleted = continuation;
            m_Task.Status = MyTaskStatus.Complete;

            if (m_Task.Parent != null)
                m_Task.Parent.Status = MyTaskStatus.Wait;

            m_Task.scheduler.QueueTask(m_Task);
        }
    }

    public class MyAwaiter<T> : IMyAwaiter
    {
        MyTask<T> m_Task;

        public MyTask Task => m_Task;

        public MyAwaiter(MyTask<T> task)
        {
            m_Task = task;
        }

        public bool IsCompleted => m_Task.IsCompleted;
        public T GetResult()
        {
            return m_Task.GetResult();
        }
        public void OnCompleted(Action continuation)
        {
            m_Task.OnCompleted = continuation;

            if (m_Task.Parent != null)
                m_Task.Parent.Status = MyTaskStatus.Wait;

            m_Task.scheduler.QueueTask(m_Task);
        }
    }
}
