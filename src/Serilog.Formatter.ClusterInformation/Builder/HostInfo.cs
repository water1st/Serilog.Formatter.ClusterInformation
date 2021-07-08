using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace Serilog.Formatter.ClusterInformation.Builder
{
    class HostInfo : IHostInfo
    {
        public string GetContainerID()
        {
            var host = NetworkInterface.GetAllNetworkInterfaces()
                .Select(p => p.GetIPProperties())
                .SelectMany(p => p.UnicastAddresses)
                .FirstOrDefault(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address))?.Address.ToString();

            return host;
        }

        public string GetCurrentHost()
        {
            var hostname = Environment.GetEnvironmentVariable("HOSTNAME", EnvironmentVariableTarget.Process);
            return hostname;
        }
    }
}
