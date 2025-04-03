using EzUptime.Dtos;
using EzUptime.Services.Monitoring.Monitor;

namespace EzUptime.Services.Monitoring
{
    public class MonitoringService
    {
        ILogger<ConfigService> _logger;

        public Dictionary<string, IMonitor> Monitors { get; private set; }

        public MonitoringService(ILogger<ConfigService> logger)
        {
            _logger = logger;
            Monitors = new();
        }

        public void AddMonitor(string name, MonitoringConfigDto config)
        {
            if (Monitors.ContainsKey(name))
                throw new ArgumentException($"Monitor {name} already exists");
            
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");

            if (config == null) throw new ArgumentNullException("config");

            try
            {
                var monitor = CreateMonitor(config);
                Monitors.Add(name, monitor);
                _logger.LogInformation($"Added monitor {name}");
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Creating monitor {name} failed, {ex.Message}");
            }
        }

        public void RemoveMonitor(string name)
        {
            if (!Monitors.ContainsKey(name))
                throw new FileNotFoundException($"No monitor named: {name}");

            if (Monitors.Remove(name, out var monitor))
            {
                monitor.StopMonitoring();
                _logger.LogInformation($"Removed monitor {name}");
            }
        }

        private IMonitor CreateMonitor(MonitoringConfigDto config)
        {
            IMonitor monitor;
            switch (config.Type)
            {
                default:
                    throw new ArgumentNullException($"Wrong monitor type {config.Type}");
                case MonitorType.HttpGet: 
                    monitor = new HttpMonitor(); break;
                case MonitorType.Ping:
                    monitor = new PingMonitor(); break;
            }
            monitor.StartMonitoring(config);
            return monitor;
        }
    }
}
