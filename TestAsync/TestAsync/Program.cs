using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            DisplayPrimeCountsAsync();
        }

        static int GetPrimesCount(int start, int count)
        {
            return ParallelEnumerable.Range(start, count).Count((n) => {
                //Console.WriteLine($"Thread({Thread.CurrentThread.ManagedThreadId})");
                return Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0);
            });
        }

        static void DisplayPrimeCounts()
        {
            for (int i = 0; i < 10; i++)
                Console.WriteLine(
                    GetPrimesCount(i * 1000000 + 2, 1000000) + " primes between " + (i * 100000) + " and " + ((i + 1) * 1000000 - 1)
                    );

            Console.WriteLine("Done!");
        }

        static Task<int> GetPrimeCountAsync(int start, int count)
        {
            return Task.Run(() => GetPrimesCount(start, count));
        }

        static void DisplayPrimeCountsAsync()
        {
            for (int i = 0; i < 10; i++)
            {
                var awaiter = GetPrimeCountAsync(i * 1000000 + 2, 1000000).GetAwaiter();
                awaiter.OnCompleted(() =>
                    Console.WriteLine(
                        awaiter.GetResult() + " primes between " + (i * 100000) + " and " + ((i + 1) * 1000000 - 1)
                        )
                );

                Console.WriteLine($"i = {i}");
            }

            Console.WriteLine("Done!");
        }
    }
}
