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
using System.IO;

namespace Manatee.Json.Test
{
    internal static class Program
    {
        private class Table
        {
            [Range(10, 100)]
            public int a;
        }

        private class TableCollection
        {
            public Dictionary<int, Table> tables = new Dictionary<int, Table>();
        }

        private static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            TableCollection tc = new TableCollection();
            tc.tables.Add(1, new Table(){a = 1});
            tc.tables.Add(2, new Table(){a = 2});

            string json = JsonConvert.SerializeObject(tc);

            File.WriteAllText("tables.json", json);

            Console.WriteLine("json----------\n" + json);

            XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(json);
            doc.Save("tables.xml");

            string json2 = JsonConvert.SerializeXmlNode(doc);

            Console.WriteLine("json2----------\n" + json2);

            JSchemaGenerator gen = new JSchemaGenerator();
            JSchema schema = gen.Generate(typeof(TableCollection));
            using (StreamWriter sw = new StreamWriter("tables.schema"))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    schema.WriteTo(writer);
                }
            }

            
            Console.WriteLine("tables.schema----------\n" + schema.ToString());

            Console.WriteLine("Hello World!");
        }
    }
}
