using System;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema.Generation;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Manatee.Json.Test
{
    class Program
    {
        class Table
        {
            [Range(10, 100)]
            public int a;
        }

        class TableCollection
        {
            public Dictionary<int, Table> tables = new Dictionary<int, Table>();
        }
        static void Main(string[] args)
        {
            
            string json = JsonConverter

            Console.WriteLine("Hello World!");
        }
    }
}
