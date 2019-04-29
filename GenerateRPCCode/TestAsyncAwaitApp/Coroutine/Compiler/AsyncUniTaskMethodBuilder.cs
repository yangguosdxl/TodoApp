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
            m_Task.Status = MyTaskStatus.Exception;
        }

        // 4. SetResult
        
        public void SetResult()
        {
            m_Task.Status = MyTaskStatus.Complete;
        }

        // 5. AwaitOnCompleted
        
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : IMyAwaiter
            where TStateMachine : IAsyncStateMachine
        {
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

            if (MyTask.Current != null)
            {
                m_Task.Parent = MyTask.Current;
                MyTask.Current = m_Task;
                
            }              

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
        
        public static AsyncUniTaskMethodBuilder Create()
        {
            var builder = new AsyncUniTaskMethodBuilder();
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
            m_Task.Status = MyTaskStatus.Exception;
        }

        // 4. SetResult
        
        public void SetResult(ref T result)
        {
            m_Task.SetResult(ref result);
            m_Task.Status = MyTaskStatus.Complete;
        }

        // 5. AwaitOnCompleted
        
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : IMyAwaiter
            where TStateMachine : IAsyncStateMachine
        {
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