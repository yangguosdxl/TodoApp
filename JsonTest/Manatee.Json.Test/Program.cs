using System;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema.Generation;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System.Xml;

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
            TableCollection tc = new TableCollection();

            string json = JsonConvert.SerializeObject(tc);

            Console.WriteLine(json);

            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(json);

            Console.WriteLine(doc.ToString());

            Console.WriteLine("Hello World!");
        }
    }
}
