using GrainInterface;
using GrainsTest;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ServerTest
{
    class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "RpcTest";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(MessageRouter).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .AddMemoryGrainStorageAsDefault()
                //.AddAdoNetGrainStorage("Mysql", options =>
                //{
                //    options.Invariant = "MySql.Data.MySqlClient";
                //    options.ConnectionString = "server=localhost;user id=root;password=123456;database=TodoTest;pooling=true;SslMode=none;";
                //    options.UseJsonFormat = true;
                //})
                .UseDashboard(options => { })
                ;

            var host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}
