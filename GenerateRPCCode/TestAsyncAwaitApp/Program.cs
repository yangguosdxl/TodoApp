using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestAsyncAwaitApp.Coroutine;

namespace TestAsyncAwaitApp
{
    class Program
    {
        struct StructA
        {

        }
        static StructA[] alist = new StructA[10];

        static WaitOneFrameTasks s_WaitOneFrameTasks = new WaitOneFrameTasks();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CustomTaskScheduler scheduler = new CustomTaskScheduler();
            MySynchronizationContext context = new MySynchronizationContext();

            MyTaskScheduler myTaskScheduler = new MyTaskScheduler();

            //SynchronizationContext.SetSynchronizationContext(context);

            //Task.Factory.StartNew(Hello, "A:", new CancellationToken(), TaskCreationOptions.None, scheduler);
            //Task.Factory.StartNew(Hello, "B:", new CancellationToken(), TaskCreationOptions.None, scheduler);
            //Task.Factory.StartNew(Hello, "C:", new CancellationToken(), TaskCreationOptions.None, scheduler);

            // Task.Factory.StartNew(Hello, "PoolA:", new CancellationToken(), TaskCreationOptions.None, TaskScheduler.Default);
            //Task.Factory.StartNew(Hello, "PoolB:", new CancellationToken(), TaskCreationOptions.None, TaskScheduler.Default);
            //Task.Factory.StartNew(Hello, "PoolC:", new CancellationToken(), TaskCreationOptions.None, TaskScheduler.Default);

            //Hello("E");

            MyTask.Run(HelloMyTask, "I");
            //MyTask.Run(HelloMyTask2, "有返回值：");

            Log("Main");

            while(Console.KeyAvailable == false)
            {
                scheduler.Update();
                //myTaskScheduler.Update();
                s_WaitOneFrameTasks.Update();
                //context.Update();
            }

            ref StructA a = ref alist[0];
        }

        static async MyTask<string> HelloMyTask2(object prefix)
        {
            for (int i = 0; i < 3; i++)
            {
                Log(prefix + $"{i}");
                await MyTaskDelay(1000);
            }

            return "我是返回了String的协程";
        }

        static async MyTask HelloMyTask(object prefix)
        {
            string ret = await HelloMyTask2("我是有返回值的: ");

            Console.WriteLine(ret);

            for (int i = 0; i < 3; i++)
            {
                Log(prefix + $"{i}");
                await MyTaskDelay(1000);
            }
        }

        static async MyTask MyTaskDelay(int t)
        {
            for (int n = t / 500, i = 0; i < n; ++i)
            {
                await s_WaitOneFrameTasks.WaitOneFrame();
                Log($"delay time {i * 10}");
                //throw new Exception($"EXCEPTION!! delay time {i * 10}");
            }
        }

        static async Task Hello(object prefix)
        {
            for(int i = 0; i < 3; i++)
            {
                Log(prefix + $"{i}");
                await Delay(1000);
            }
           
        }

        static void Log(string format, params object[] args)
        {
            string szTaskScheduler = TaskScheduler.Current is CustomTaskScheduler ? "My" : "Default";
            string context = SynchronizationContext.Current is MySynchronizationContext ? "My" : "" + SynchronizationContext.Current;
            Console.WriteLine($"Context: {context}, TaskScheduler: {szTaskScheduler}, ThreadID: {Thread.CurrentThread.ManagedThreadId}. " + format, args);
        }

        static async Task Delay(int t)
        {
            for(int n = t/10, i = 0; i < n; ++i)
            {
                await Task.Yield();
                Log($"delay time {i*10}");
            }
        }
    }
}
