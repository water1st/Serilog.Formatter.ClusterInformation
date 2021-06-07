using Newtonsoft.Json;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;

namespace Serilog.Formatter.ClusterInformation.Formatters
{
    public class ClusterInformationFormatter : ITextFormatter
    {
        private readonly static ITextFormatter instance = new ClusterInformationFormatter();
        public static ITextFormatter Instance => instance;

        public void Format(LogEvent logEvent, TextWriter output)
        {
            var body = new LogBody();
            body.Timestamp = logEvent.Timestamp.LocalDateTime;
            body.Level = logEvent.Level.ToString();
            body.ApplicationName = logEvent.Properties[nameof(body.ApplicationName)]?.ToString();
            body.Host = logEvent.Properties[nameof(body.Host)]?.ToString();
            body.ContainerId = logEvent.Properties[nameof(body.ContainerId)]?.ToString();
            body.Environment = logEvent.Properties[nameof(body.Environment)]?.ToString();
            body.Source = logEvent.Properties["SourceContext"]?.ToString();
            body.Version = logEvent.Properties[nameof(body.Version)]?.ToString();
            body.Message = logEvent.MessageTemplate.Render(logEvent.Properties);
            body.Exception = logEvent.Exception;

            var json = JsonConvert.SerializeObject(body);

            output.WriteLine(json);
        }


        private class LogBody
        {
            public DateTimeOffset Timestamp { get; set; }
            public string Level { get; set; }
            public string ApplicationName { get; set; }
            public string Host { get; set; }
            public string ContainerId { get; set; }
            public string Environment { get; set; }
            public string Source { get; set; }
            public string Message { get; set; }
            public string Version { get; set; }
            public Exception Exception { get; set; }
        }
    }
}
