using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UniRx.Async.CompilerServices;

namespace TestAsyncAwaitApp.Coroutine
{
    public enum MyTaskStatus
    {
        None,
        Active,
        Complete,
        Canceled,
        Exception,
    }
    [AsyncMethodBuilder(typeof(AsyncUniTaskMethodBuilder))]
    public class MyTask
    {
        public static MyTask Run(Action action, MyTask parent, MyTaskScheduler scheduler)
        {
            MyTask myTask = new MyTask();
            myTask.Action = action;
            myTask.Parent = parent;

            scheduler.QueueTask(myTask);

            return myTask;
        }

        List<Exception> m_Exceptions = new List<Exception>();

        public MyTask Parent { get; set; }

        public MyTaskStatus Status { get; set; }

        public Action OnCompleted { get; set; }

        public Action Action { get; set; }

        public bool IsCompleted => Status == MyTaskStatus.Complete;

        public MyAwaiter GetAwaiter()
        {
            return new MyAwaiter(this);
        }

        public void SetResult()
        {
        }

        public void GetResult()
        {
        }

        public AggregateException GetException()
        {
            return new AggregateException(m_Exceptions);
        }

        public void SetException(Exception e)
        {
            if (Parent != null)
                Parent.SetException(e);
            else
                m_Exceptions.Add(e);
        }
    }

    [AsyncMethodBuilder(typeof(AsyncUniTaskMethodBuilder))]
    public class MyTask<T> : MyTask
    {
        T m_Result;

        public new MyAwaiter<T> GetAwaiter()
        {
            return new MyAwaiter<T>(this);
        }

        public void SetResult(ref T result)
        {
            m_Result = result;
        }

        public new T GetResult()
        {
            return m_Result;
        }
    }
}
