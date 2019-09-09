using System;
using System.Collections.Generic;

namespace Manatee.Json.Test
{
    class Program
    {
        class Table
        {
            public int a;
        }

        class TableCollection
        {
            public Dictionary<int, Table> tables = new Dictionary<int, Table>();
        }
        static void Main(string[] args)
        {
            
            var serializer = new JsonSerializer();

            Console.WriteLine("Hello World!");
        }
    }
}
