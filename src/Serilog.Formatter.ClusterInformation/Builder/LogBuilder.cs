namespace Serilog.Formatter.ClusterInformation.Builder
{
    class LogBuilder : ILogBuilder
    {
        private readonly LoggerConfiguration loggerConfiguration;
        private readonly ClusterInformation logOptions;
        public LogBuilder(IHostInfo hostInfo)
        {
            loggerConfiguration = new LoggerConfiguration();
            logOptions = new ClusterInformation();
            HostInfo = hostInfo;
        }

        public LoggerConfiguration LoggerConfiguration => loggerConfiguration;
        public ClusterInformation LogOptions => logOptions;
        public IHostInfo HostInfo { get; }
    }
}
