using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Serilog.Formatter.ClusterInformation.Formatters
{
    public class ClusterInformationFormatter : ITextFormatter
    {
        private readonly static ITextFormatter instance = new ClusterInformationFormatter();
        public static ITextFormatter Instance => instance;

        public void Format(LogEvent logEvent, TextWriter output)
        {
            output.Write('{');
            WriteQuotedValueJson("Timestamp", logEvent.Timestamp.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff zzz"), output);
            WriteQuotedValueJson("Level", logEvent.Level.ToString(), output);
            WriteJson("ApplicationName", logEvent.Properties["ApplicationName"]?.ToString(), output);
            WriteJson("Host", logEvent.Properties["Host"]?.ToString(), output);
            WriteJson("ContainerId", logEvent.Properties["ContainerId"]?.ToString(), output);
            WriteJson("Environment", logEvent.Properties["Environment"]?.ToString(), output);
            WriteJson("Source", logEvent.Properties["SourceContext"]?.ToString(), output);
            WriteJson("Version", logEvent.Properties["Version"]?.ToString(), output);
            WriteQuotedValueJson("Message", logEvent.MessageTemplate.Render(logEvent.Properties), output);
            WriteJson("Exception", GetExceptionInfo(logEvent.Exception), output);
            output.Write('}');
        }

        private string GetExceptionInfo(Exception exception)
        {
            if (exception == null)
                return "null";

            var stringBuilder = new StringBuilder();
            using (var stringWriter = new StringWriter(stringBuilder))
            {
                stringWriter.Write('{');
                WriteQuotedValueJson(nameof(exception.Message), exception.Message, stringWriter);
                WriteQuotedValueJson(nameof(exception.Source), exception.Source, stringWriter);
                WriteQuotedValueJson(nameof(exception.StackTrace), exception.StackTrace, stringWriter);
                WriteJson(nameof(exception.Data), GetExceptionData(exception.Data), stringWriter);
                WriteJson(nameof(exception.InnerException), GetExceptionInfo(exception.InnerException), stringWriter);
                stringWriter.Write('}');
            }

            var result = stringBuilder.ToString();
            return result;
        }

        private string GetExceptionData(IDictionary dictionary)
        {
            if (dictionary == null)
                return "null";

            var stringBuilder = new StringBuilder();
            using (var stringWriter = new StringWriter(stringBuilder))
            {
                stringWriter.Write('{');
                foreach (var key in dictionary.Keys)
                {
                    var value = dictionary[key];
                    WriteQuotedValueJson(key.ToString(), value.ToString(), stringWriter);
                }
                stringWriter.Write('}');
            }
            var result = stringBuilder.ToString();
            return result;
        }

        private void WriteQuotedValueJson(string key, string quotedValue, TextWriter output)
        {
            JsonValueFormatter.WriteQuotedJsonString(key, output);
            output.Write(':');
            if (string.IsNullOrEmpty(quotedValue))
            {
                quotedValue = "null";
                output.Write(quotedValue);
            }
            else
            {
                JsonValueFormatter.WriteQuotedJsonString(quotedValue, output);
            }
        }

        private void WriteJson(string key, string value, TextWriter output)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = "null";
            }
            JsonValueFormatter.WriteQuotedJsonString(key, output);
            output.Write(':');
            output.Write(value);
        }
    }
}
