using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorld.Middleware
{
    public class Ping : System.Net.NetworkInformation.Ping
    {
        public Task<PingReply> sendPingAsync(string _hostname, CancellationTokenSource _cancellationTokenSource)
        {
            string hostname = _hostname;
            CancellationTokenSource cancellationTokenSource = _cancellationTokenSource;

            Task<PingReply> pingReply = null;

            return pingReply;
        }
    }
}