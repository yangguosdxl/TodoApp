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
        Wait,
        Complete,
        Canceled,
        Exception,
    }
    [AsyncMethodBuilder(typeof(AsyncUniTaskMethodBuilder))]
    public class MyTask
    {
        public static MyTask Current;
        public static int NextTaskID { get; set; }

        public static MyTask Run(Action action, MyTask parent, MyTaskScheduler scheduler)
        {
            MyTask myTask = new MyTask();
            myTask.szName = "RunAction" + action;
            myTask.Action = action;
            myTask.Parent = parent;
            myTask.scheduler = scheduler;

            Current = myTask;

            if (action != null)
            {
                action();
            }

            scheduler.QueueTask(myTask);

            return myTask;
        }

        public static WaitOneFrameTask WaitOneFrame()
        {
            WaitOneFrameTask task = new WaitOneFrameTask();
            task.szName = "WaitOneFrameTask";
            task.Parent = MyTask.Current;
            MyTask.Current = task.Parent;
            return task;
        }

        public int iTaskID { get; set; }
        public string szName { get; set; }

        List<Exception> m_Exceptions = new List<Exception>();

        private MyTask m_Parent;
        public MyTask Parent
        {
            get => m_Parent;
            set
            {
                m_Parent = value;
                scheduler = value?.scheduler;

                //Console.WriteLine($"------------TaskName: {szName}, TaskID: {iTaskID}, Parent: {m_Parent?.iTaskID}");
            }
        }

        MyTaskStatus m_Status;
        public MyTaskStatus Status
        {
            get => m_Status;
            set
            {
                m_Status = value;

                Console.WriteLine(this);
            }
        }

        public Action OnCompleted { get; set; }

        public Action Action { get; set; }

        public MyTaskScheduler scheduler { get; set; }

        public bool IsCompleted => Status == MyTaskStatus.Complete || Status == MyTaskStatus.Canceled || Status == MyTaskStatus.Exception;

        public MyTask()
        {
            iTaskID = ++MyTask.NextTaskID;

            Status = MyTaskStatus.Active;
        }
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

        public override string ToString()
        {
            return $"TaskName: {szName}, TaskID: {iTaskID}, Parent: {Parent?.iTaskID}, Status: {Status}";
        }
    }

    [AsyncMethodBuilder(typeof(AsyncUniTaskMethodBuilder))]
    public class WaitOneFrameTask : MyTask
    {
        public new WaitOneFrameAwaiter GetAwaiter()
        {
            return new WaitOneFrameAwaiter(this);
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
