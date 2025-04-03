using EzUptime.Dtos;

namespace EzUptime.Services.Monitoring
{
    public interface IMonitor
    {
        void StartMonitoring(MonitoringConfigDto config);
        void StopMonitoring();
        MonitoringHistoryDto GetHistory();
        EventHandler OnError { get; }
    }
}
