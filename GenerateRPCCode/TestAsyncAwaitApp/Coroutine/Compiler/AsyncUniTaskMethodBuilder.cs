#if CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NET_STANDARD_2_0 || NET_4_6))

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
        [DebuggerHidden]
        public static AsyncUniTaskMethodBuilder Create()
        {
            var builder = new AsyncUniTaskMethodBuilder();
            return builder;
        }

        // 2. TaskLike Task property.
        [DebuggerHidden]
        public MyTask Task
        {
            get
            {
                m_Task = new MyTask();
                return m_Task;
            }
        }

        // 3. SetException
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            m_Task.SetException(exception);
            m_Task.Status = MyTaskStatus.Exception;
        }

        // 4. SetResult
        [DebuggerHidden]
        public void SetResult()
        {
            m_Task.Status = MyTaskStatus.Complete;
        }

        // 5. AwaitOnCompleted
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            var runner = new MoveNextRunner<TStateMachine>();
            runner.StateMachine = stateMachine;
            awaiter.OnCompleted(runner.Run);
        }
        
        // 7. Start
        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        // 8. SetStateMachine
        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }


    public struct AsyncUniTaskMethodBuilder<T>
    {
        MyTask<T> m_Task;

        // 1. Static Create method.
        [DebuggerHidden]
        public static AsyncUniTaskMethodBuilder Create()
        {
            var builder = new AsyncUniTaskMethodBuilder();
            return builder;
        }

        // 2. TaskLike Task property.
        [DebuggerHidden]
        public MyTask<T> Task
        {
            get
            {
                m_Task = new MyTask<T>();
                return m_Task;
            }
        }

        // 3. SetException
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            m_Task.SetException(exception);
            m_Task.Status = MyTaskStatus.Exception;
        }

        // 4. SetResult
        [DebuggerHidden]
        public void SetResult(ref T result)
        {
            m_Task.SetResult(ref result);
            m_Task.Status = MyTaskStatus.Complete;
        }

        // 5. AwaitOnCompleted
        [DebuggerHidden]
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            var runner = new MoveNextRunner<TStateMachine>();
            runner.StateMachine = stateMachine;
            awaiter.OnCompleted(runner.Run);
        }

        // 7. Start
        [DebuggerHidden]
        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        // 8. SetStateMachine
        [DebuggerHidden]
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}

#endif