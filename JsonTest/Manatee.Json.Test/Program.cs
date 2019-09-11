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
using System.Xml.Serialization;
using System.Text;

namespace Manatee.Json.Test
{
    public static class Program
    {
        public class Table
        {
            [Range(10, 100)]
            public int a;
        }

        public class TableCollection
        {
            public List<Table> tables = new List<Table>();
        }

        private static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            TableCollection tc = new TableCollection();
            tc.tables.Add(new Table(){a = 1});
            tc.tables.Add(new Table(){a = 2});

            var xmlSerializer = new XmlSerializer(tc.tables.GetType());
            var str = new StringBuilder();

            xmlSerializer.Serialize(new StringWriter(str), tc.tables);

            var xml = str.ToString();
            File.WriteAllText("tables.xml", xml);

            Console.WriteLine("xml----------\n" + xml);

            string json = JsonConvert.SerializeObject(tc.tables);

            File.WriteAllText("tables.json", json);

            Console.WriteLine("json----------\n" + json);

            //XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(json);
            //doc.Save("tables.xml");

            //string json2 = JsonConvert.SerializeXmlNode(doc);

            //Console.WriteLine("json2----------\n" + json2);

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
