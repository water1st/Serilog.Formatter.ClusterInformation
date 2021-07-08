namespace Serilog.Formatter.ClusterInformation.Builder
{
    public interface IHostInfo
    {
        string GetContainerID();
        string GetCurrentHost();
    }
}
