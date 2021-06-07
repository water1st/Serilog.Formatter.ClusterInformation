namespace Serilog.Formatter.ClusterInformation.Builder
{
    public class ClusterInformation
    {
        public string ApplicationName { get; set; }
        public string ContainerId { get; set; }
        public string Host { get; set; }
        public string Environment { get; set; }
        public string Version { get; set; }
    }
}
