using EzUptime.Dtos;
using System.Net.NetworkInformation;
using System.Net;
using EzUptime.Utils;

namespace EzUptime.Services.Monitoring.Monitor
{
    public class PingMonitor : IMonitor
    {
        MonitoringConfigDto _config { get; set; }

        List<MonitoringStepDto> _history { get; set; } = new();

        CancellationTokenSource _cts;

        public EventHandler OnError { get; set; }

        DateTime _created;
        
        public MonitoringHistoryDto GetHistory()
        {
            return new MonitoringHistoryDto()
            {
                ConfigDto = _config,
                Created = DateTime.UtcNow,
                Resutls = _history
            };
        }

        public void StartMonitoring(MonitoringConfigDto config)
        {
            if (_cts != null && !_cts.IsCancellationRequested)
                StopMonitoring();
            
            if (!IPAddress.TryParse(config.Address, out _))
                throw new Exception($"error parsing ip address for config {config.Label} - {config.Address}");

            _created = DateTime.UtcNow;
            _cts = new CancellationTokenSource();
            _config = config;
            Task.Run(MonitoringWorker);
        }

        public void StopMonitoring() => _cts.Cancel();

        async Task MonitoringWorker()
        {
            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(_config.Period));
                if (PingUtil.TryPingIp(_config.Address, out var result, 4, 2000))
                {
                    _history.Add(new MonitoringStepDto()
                    {
                        Timestamp = DateTime.UtcNow,
                        Ping = result.AvgPing,
                        Success = true
                    });
                }
                else
                {
                    _history.Add(new MonitoringStepDto()
                    {
                        Timestamp = DateTime.UtcNow,
                        Success = false
                    });
                    if (OnError != null)
                        OnError(this, EventArgs.Empty);
                }

                if (_config.ResultsCap !=null && _history.Count > _config.ResultsCap)
                {
                    _history.RemoveAt(0);
                }
            }
        }
    }
}
