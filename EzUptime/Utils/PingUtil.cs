using System.Net.NetworkInformation;
using System.Net;

namespace EzUptime.Utils
{
    public class PingResult
    {
        public int NumTries { get; set; }
        public int SuccessfulTries { get; set; }
        public double AvgPing { get; set; }
    }

    public class PingUtil
    {
        public static bool TryPingIp(string ip, out PingResult result, int numTries = 1, int timeoutMs = 1000)
        {
            if (!IPAddress.TryParse(ip, out _))
                throw new ArgumentException(nameof(ip));


            result = new PingResult()
            {
                NumTries = numTries,
                SuccessfulTries = 0
            };

            long totalRoundtripTime = 0;

            using (Ping pingSender = new Ping())
            {
                for (int i = 0; i < numTries; i++)
                {
                    PingReply reply = pingSender.Send(ip, timeoutMs);
                    if (reply.Status == IPStatus.Success)
                    {
                        totalRoundtripTime += reply.RoundtripTime;
                        result.SuccessfulTries++;
                    }
                }
            }

            if (result.SuccessfulTries > 0)
                result.AvgPing = totalRoundtripTime / result.SuccessfulTries;

            return result.SuccessfulTries > 0;
        }
    }
}
