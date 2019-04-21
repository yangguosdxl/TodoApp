using GrainInterface;
using MyNetWork;
using NetWorkInterface;
using Orleans;
using Orleans.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Gateway
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = new ClientBuilder()
                        // Clustering information
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "my-first-cluster";
                            options.ServiceId = "MyOrleansService";
                        })
                        // Clustering provider
                        //.UseAzureStorageClustering(options => options.ConnectionString = connectionString)
                        // Application parts: just reference one of the grain interfaces that we use
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IClientSession).Assembly))
                        .Build();

            client.Connect().GetAwaiter().GetResult();

            DefaultNetListener listener = new DefaultNetListener();
            listener.Init(new IPEndPoint(IPAddress.Any, 1234), NetType.TCP);

            listener.OnNewConnection += OnNewConnection;
            listener.Startup();

            while(true)
            {

            }

        }

        private static void OnNewConnection(ISocketTask obj)
        {
            throw new NotImplementedException();
        }
    }
}
