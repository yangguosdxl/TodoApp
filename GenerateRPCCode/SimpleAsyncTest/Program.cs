using System;
using System.Threading.Tasks;

namespace SimpleAsyncTest
{
    class Type1 { }
    class Type2 { }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        async static Task<Type1> Method1Async()
        {
            return new Type1();
        }

        async static Task<Type2> Method2Async()
        {
            return new Type2();
        }

        static async Task<String> MyMethodAsync(int argument)
        {
            int local = argument;

            try
            {
                Type1 result1 = await Method1Async();
                for (int x = 0; x < 3; x++)
                {
                    Type2 result2 = await Method2Async();
                }
                    
            }
            catch(Exception)
            {

            }
            finally
            {

            }
            return "Done";
        }
    }
}
