using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace BeComfy.ApiGateway.Ocelot
{
    public class Program
    {
        public static Task Main(string[] args) 
            => CreateHostBuilder(args).Build().RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) => 
                {
                    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json")
                        .AddJsonFile($"ocelot.json")
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHostDefaults(
                    builder => builder.Configure(app => 
                    {
                        app.UseOcelot();
                    })
                    .ConfigureServices(s => s.AddOcelot())
                );
    }
}
