using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace TestAsyncAwaitApp.Coroutine
{
    public class MyAwaiter : INotifyCompletion
    {
        MyTask m_Task;

        public bool IsCompleted { get; set; }

        public MyAwaiter(MyTask task)
        {
            m_Task = task;
        }

        public void OnCompleted(Action continuation)
        {
            m_Task.OnCompleted = continuation;
        }
    }

    public class MyAwaiter<T> : MyAwaiter
    {
        MyTask<T> m_Task;

        public MyAwaiter(MyTask<T> task)
        {
            m_Task = task;
        }

        public bool IsCompleted { get; }
        public T GetResult()
        {
            return m_Task.get
        }
        public void OnCompleted(Action completion);
    }
}
