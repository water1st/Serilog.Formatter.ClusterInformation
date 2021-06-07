using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Serilog.Formatter.ClusterInformation.Sample
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            IConfiguration configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddSerilog(configuration);

            var provider = services.BuildServiceProvider();
            var logger = provider.GetService<ILogger<Program>>();

            var err1 = new ArgumentException("Critical Exception");
            var err2 = new ArgumentNullException("Error Exception");
            var err3 = new ApplicationException("Warning Exception");

            while (true)
            {
                Console.WriteLine("input your log,if you want to quit please input the \"quit\"");
                var log = Console.ReadLine();
                if (log == "quit")
                    break;

                logger.LogTrace($"Log the Trace {log}");
                logger.LogDebug($"Log the Debug {log}");
                logger.LogInformation($"Log the Information {log}");
                logger.LogWarning(err1, $"Log the Warning {log}");
                logger.LogError(err2, $"Log the Error {log}");
                logger.LogCritical(err3, $"Log the Critical {log}");
            }
        }
    }
}
