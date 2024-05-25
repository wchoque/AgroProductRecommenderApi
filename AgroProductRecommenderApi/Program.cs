using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace AgroProductRecommenderApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var settings = config.Build();
                    var connectionString = "Endpoint=https://lm-runtime-p-aze2-appcs-001.azconfig.io;Id=mf65;Secret=BU42DwR60eF4vsAVojH6NKK3QZkXRns0d43bo6cAjf4=";

                    // Agrega Azure App Configuration
                    config.AddAzureAppConfiguration(connectionString);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}