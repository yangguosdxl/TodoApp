using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsync
{
    class Program2
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CountdownEvent countdown = new CountdownEvent(1);

            for(int i = 0; i < 10; ++i)
            {
                countdown.AddCount(1);
                Task.Factory.StartNew((state) =>
                {
                    Console.WriteLine($"work i: {(int)state}, ThreadID {Thread.CurrentThread.ManagedThreadId}");
                    countdown.Signal();
                }, i);
            }

            countdown.Signal();
            countdown.Wait();

            Console.WriteLine("press any key");

            Console.ReadKey();
        }
    }
}
