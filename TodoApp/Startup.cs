using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Todo.Interface;

namespace TodoApp
{
    public class Startup
    {
        public static IClusterClient SlioClient { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            SlioClient = new ClientBuilder()
               .UseLocalhostClustering()
               .Configure<ClusterOptions>(options =>
               {
                   options.ClusterId = "dev";
                   options.ServiceId = "TodoApp";
               })
               .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IUserGrain).Assembly).WithReferences())
               .Build();

            RunAsync().Wait();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        static async Task RunAsync()
        {
            await SlioClient.Connect();
        }
    }
}
