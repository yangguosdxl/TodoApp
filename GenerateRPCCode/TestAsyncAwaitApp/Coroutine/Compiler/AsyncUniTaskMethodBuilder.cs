#if true// CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NET_STANDARD_2_0 || NET_4_6))

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;
using TestAsyncAwaitApp.Coroutine;

namespace UniRx.Async.CompilerServices
{
    public struct AsyncUniTaskMethodBuilder
    {
        MyTask m_Task;

        // 1. Static Create method.
        
        public static AsyncUniTaskMethodBuilder Create()
        {
            var builder = new AsyncUniTaskMethodBuilder();
            builder.m_Task = new MyTask();
            return builder;
        }

        // 2. TaskLike Task property.
        
        public MyTask Task
        {
            get
            {
                return m_Task;
            }
        }

        // 3. SetException
        
        public void SetException(Exception exception)
        {
            m_Task.SetException(exception);
        }

        // 4. SetResult
        
        public void SetResult()
        {
            m_Task.SetResult();
        }

        // 5. AwaitOnCompleted
        
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : IMyAwaiter
            where TStateMachine : IAsyncStateMachine
        {
            m_Task.Child = awaiter.Task;

            var runner = new MoveNextRunner<TStateMachine>();
            runner.StateMachine = stateMachine;
            awaiter.OnCompleted(runner.Run);
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : IMyAwaiter
            where TStateMachine : IAsyncStateMachine
        {
            AwaitOnCompleted(ref awaiter, ref stateMachine);
        }

        // 7. Start
        
        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            m_Task.szName = stateMachine.ToString();

            stateMachine.MoveNext();
        }

        // 8. SetStateMachine
        
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }


    public struct AsyncUniTaskMethodBuilder<T>
    {
        MyTask<T> m_Task;

        // 1. Static Create method.
        
        public static AsyncUniTaskMethodBuilder<T> Create()
        {
            var builder = new AsyncUniTaskMethodBuilder<T>();
            return builder;
        }

        // 2. TaskLike Task property.
        
        public MyTask<T> Task
        {
            get
            {
                m_Task = new MyTask<T>();
                return m_Task;
            }
        }

        // 3. SetException
        
        public void SetException(Exception exception)
        {
            m_Task.SetException(exception);
        }

        // 4. SetResult
        
        public void SetResult(T result)
        {
            m_Task.SetResult(ref result);
        }

        // 5. AwaitOnCompleted
        
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : IMyAwaiter
            where TStateMachine : IAsyncStateMachine
        {
            m_Task.Child = awaiter.Task;

            var runner = new MoveNextRunner<TStateMachine>();
            runner.StateMachine = stateMachine;
            awaiter.OnCompleted(runner.Run);
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : IMyAwaiter
            where TStateMachine : IAsyncStateMachine
        {
            AwaitOnCompleted(ref awaiter, ref stateMachine);
        }

        // 7. Start
        
        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        // 8. SetStateMachine
        
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}

#endif