namespace Serilog.Formatter.ClusterInformation.Builder
{
    class LogBuilder : ILogBuilder
    {
        private readonly LoggerConfiguration loggerConfiguration;
        private readonly ClusterInformation logOptions;
        public LogBuilder()
        {
            loggerConfiguration = new LoggerConfiguration();
            logOptions = new ClusterInformation();
        }

        public LoggerConfiguration LoggerConfiguration => loggerConfiguration;
        public ClusterInformation LogOptions => logOptions;
    }
}
