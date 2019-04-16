using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsyncAwaitApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            MyTaskScheduler scheduler = new MyTaskScheduler();
            MySynchronizationContext context = new MySynchronizationContext();

            SynchronizationContext.SetSynchronizationContext(context);

            Task.Factory.StartNew(Hello, "A:", new CancellationToken(), TaskCreationOptions.None, scheduler);
            //Task.Factory.StartNew(Hello, "B:", new CancellationToken(), TaskCreationOptions.None, scheduler);
            //Task.Factory.StartNew(Hello, "C:", new CancellationToken(), TaskCreationOptions.None, scheduler);

            //Task.Factory.StartNew(Hello, "PoolA:", new CancellationToken(), TaskCreationOptions.None, TaskScheduler.Default).ConfigureAwait(false);
            //Task.Factory.StartNew(Hello, "PoolB:", new CancellationToken(), TaskCreationOptions.None, TaskScheduler.Default);
            //Task.Factory.StartNew(Hello, "PoolC:", new CancellationToken(), TaskCreationOptions.None, TaskScheduler.Default);

            //Hello("E");

            Log("Main");

            while(Console.KeyAvailable == false)
            {
                scheduler.Update();
                context.Update();
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
            string szTaskScheduler = TaskScheduler.Current is MyTaskScheduler ? "My" : "Default";
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
