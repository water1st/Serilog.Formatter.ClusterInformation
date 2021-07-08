namespace Serilog.Formatter.ClusterInformation.Builder
{
    public interface ILogBuilder
    {
        LoggerConfiguration LoggerConfiguration { get; }
        ClusterInformation LogOptions { get; }
        IHostInfo HostInfo { get; }
    }
}