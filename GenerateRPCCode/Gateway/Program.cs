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
        static IClusterClient client;
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = new ClientBuilder()
                        // Clustering information
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = "dev";
                            options.ServiceId = "RpcTest";
                        })
                        // Clustering provider
                        //.UseAzureStorageClustering(options => options.ConnectionString = connectionString)
                        // Application parts: just reference one of the grain interfaces that we use
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IClientSessionGrain).Assembly))
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

        private static void OnNewConnection(ISocketTask socket)
        {
            ClientSession session = new ClientSession(socket, Guid.NewGuid(), client);
            SessionMgr.Inst.TryAdd(session.SessionID, session);
        }
    }
}
