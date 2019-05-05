﻿using System;
using System.Runtime.CompilerServices;

namespace Cool.Coroutine
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
        }
    }
}