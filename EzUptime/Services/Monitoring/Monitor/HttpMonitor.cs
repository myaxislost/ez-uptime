using EzUptime.Dtos;

namespace EzUptime.Services.Monitoring.Monitor
{
    public class HttpMonitor : IMonitor
    {
        public MonitoringConfigDto _config;
        public List<MonitoringStepDto> _history = new();
        DateTime _created;

        public EventHandler OnError { set; get; }

        CancellationTokenSource _cts;
        HttpClient cli;

        public MonitoringHistoryDto GetHistory()
        {
            return new MonitoringHistoryDto()
            {
                Created = _created,
                ConfigDto = _config,
                Resutls = _history
            };
        }


        public void StartMonitoring(MonitoringConfigDto config)
        {
            if (_cts != null && !_cts.IsCancellationRequested)
                StopMonitoring();
            if (!CheckUri(config.Address))
                throw new Exception($"error parsing URL for config {config.Label} - {config.Address}");

            _config = config;
            _cts = new CancellationTokenSource();
            cli = new HttpClient();
            _created = DateTime.UtcNow;
            Task.Run(MonitoringWorker);
        }

        public void StopMonitoring() => _cts.Cancel();

        private bool CheckUri(string uri)
        {
            if (Uri.TryCreate(uri, UriKind.Absolute, out var parsed))
            {
                if (parsed.Scheme == Uri.UriSchemeHttp || parsed.Scheme == Uri.UriSchemeHttps)
                    return true;
            }
            return false;
        }

        async Task MonitoringWorker()
        {
            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(_config.Period));
                try
                {
                    var requestCts = new CancellationTokenSource();
                    requestCts.CancelAfter(TimeSpan.FromSeconds(5));
                    var startTime = DateTime.UtcNow;
                    var response = await cli.GetAsync(_config.Address, requestCts.Token);
                    _history.Add(new MonitoringStepDto()
                    {
                        Success = true,
                        Ping = (DateTime.UtcNow - startTime).TotalMilliseconds,
                        Timestamp = DateTime.UtcNow
                    });
                }
                catch
                {
                    _history.Add(new MonitoringStepDto()
                    {
                        Success = false,
                        Timestamp = DateTime.UtcNow
                    });
                }
            }
        }
    }
}
