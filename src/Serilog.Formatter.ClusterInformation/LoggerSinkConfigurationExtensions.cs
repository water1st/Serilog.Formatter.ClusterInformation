using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatter.ClusterInformation.Builder;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LoggerSinkConfigurationExtensions
    {
        public static IServiceCollection AddSerilog(this IServiceCollection services, Action<ILogBuilder> config)
        {
            services.AddLogging(logBuilder =>
            {
                var builder = new LogBuilder();
                config(builder);
                builder.LoggerConfiguration
                .Enrich.WithProperty(nameof(builder.LogOptions.ApplicationName), builder.LogOptions.ApplicationName)
                .Enrich.WithProperty(nameof(builder.LogOptions.Host), builder.LogOptions.Host)
                .Enrich.WithProperty(nameof(builder.LogOptions.ContainerId), builder.LogOptions.ContainerId)
                .Enrich.WithProperty(nameof(builder.LogOptions.Environment), builder.LogOptions.Environment)
                .Enrich.WithProperty(nameof(builder.LogOptions.Version), builder.LogOptions.Version);

                var logger = builder.LoggerConfiguration.CreateLogger();
                logBuilder.ClearProviders();
                logBuilder.AddSerilog(logger);
            });

            return services;
        }

        public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration, Action<ClusterInformation> config = null)
        {
            services.AddLogging(logBuilder =>
            {

                var options = new ClusterInformation();
                if(config != null)
                {
                    config(options);
                }
                else
                {
                    configuration.GetSection("Serilog:ClusterInformation").Bind(options);
                }
                

                var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.WithProperty(nameof(options.ApplicationName), options.ApplicationName)
                    .Enrich.WithProperty(nameof(options.Host), options.Host)
                    .Enrich.WithProperty(nameof(options.ContainerId), options.ContainerId)
                    .Enrich.WithProperty(nameof(options.Environment), options.Environment)
                    .Enrich.WithProperty(nameof(options.Version), options.Version)
                    .CreateLogger();

                logBuilder.ClearProviders();
                logBuilder.AddSerilog(logger);
            });

            return services;
        }
    }
}
