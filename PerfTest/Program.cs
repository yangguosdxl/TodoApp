using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PerfTest
{
    class Program
    {
        static List<byte[]> s_Cache = new List<byte[]>();
        static int iAllocSize;
        static int iCacheSize;
        static Random random = new Random();
        static void Main(string[] args)
        {
            

            iAllocSize = Convert.ToInt32(args[0]);
            iCacheSize = Convert.ToInt32(args[1]);

            Console.WriteLine($"11 Perf test. alloc size : {iAllocSize}, cache size {iCacheSize}");

            //OutputCacheCount().GetAwaiter();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    return;
                }

                RemoveBytes();

                AddBytes();
                


                Thread.Sleep(10);

                
            }
        }

        static void RemoveBytes()
        {
            if (s_Cache.Count >= iCacheSize)
            {
                int iRemovedSize = random.Next() % s_Cache.Count;

                for (int i = 0; i < iRemovedSize; ++i)
                {
                    int index = random.Next() % (s_Cache.Count - 1);
                    s_Cache.RemoveAt(index);
                }

            }
        }

        static void AddBytes()
        {
            if (s_Cache.Count < iCacheSize)
            {
                int iAddSize = (random.Next() % iCacheSize) / 2;

                for (int i = 0; i < iAddSize; ++i)
                    s_Cache.Add(new byte[iAllocSize]);
            }
        }

        static async Task OutputCacheCount()
        {
            while(true)
            {
                await Task.Delay(5000);

                Console.WriteLine($"cache count {s_Cache.Count}");
            }
        }
    }
}
