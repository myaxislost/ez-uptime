using EzUptime.Dtos;
using EzUptime.Services.Monitoring.Monitor;

namespace EzUptime.Services.Monitoring
{
    public class MonitoringService
    {
        ILogger<ConfigService> _logger;

        public Dictionary<string, Dictionary<string, IMonitor>> Monitors { get; private set; }

        public MonitoringService(ILogger<ConfigService> logger)
        {
            _logger = logger;
            Monitors = new();
        }

        public void AddMonitor(string groupName, string name, MonitoringConfigDto config)
        {
            if (!Monitors.ContainsKey(groupName))
                Monitors.Add(groupName, new Dictionary<string, IMonitor>());

            var group = Monitors[groupName];

            if (group.ContainsKey(name))
                throw new ArgumentException($"Monitor {name} already exists");
            
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");

            if (config == null) throw new ArgumentNullException("config");

            try
            {
                var monitor = CreateMonitor(config);
                group.Add(name, monitor);
                _logger.LogInformation($"Added monitor {name}");
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Creating monitor {name} failed, {ex.Message}");
            }
        }

        public void RemoveMonitor(string groupName, string name)
        {
            if (!Monitors.ContainsKey(groupName))
                throw new FileNotFoundException($"No group named: {groupName}");

            var group = Monitors[groupName];
            if (!group.ContainsKey(name))
                throw new FileNotFoundException($"No monitor named: {name} in group {groupName}");


            if (group.Remove(name, out var monitor))
            {
                monitor.StopMonitoring();
                _logger.LogInformation($"Removed monitor {name}");
            }
         
            if (group.Count == 0)
                Monitors.Remove(groupName);
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
