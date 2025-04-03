using EzUptime.Dtos;
using EzUptime.Services.Monitoring;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace EzUptime.Services
{
    public class ConfigService
    {
        public static readonly string configName = "config.yaml";

        private IDeserializer yaml;
        ILogger<ConfigService> _logger;
        MonitoringService _monitors;
        public Dictionary<string, List<MonitoringConfigDto>> Configs { get; private set; } = new();

        public ConfigService(ILogger<ConfigService> logger, MonitoringService monitor)
        {
            _logger = logger;
            _monitors = monitor;
            yaml = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            RestoreFromConfig();
        }

        public void RestoreFromConfig()
        {
            try
            {
                var content = File.ReadAllText(configName);
                Configs = yaml.Deserialize<Dictionary<string, List<MonitoringConfigDto>>>(content);
                _logger.LogInformation($"found {Configs.Count} configs");

                foreach (var group in Configs)
                    foreach (var config in group.Value)
                    _monitors.AddMonitor(group.Key, config.Label, config);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while reading config {e.Message}");
            }
        }

        public static void GenerateExampleConfig()
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();


            Dictionary<string, List<MonitoringConfigDto>> example = new()
            {
                {
                    "Unnamed",
                    new() {
                        new MonitoringConfigDto
                        {
                            Label = "localhost",
                            Type = MonitorType.Ping,
                            Address = "127.0.0.1",
                            Period = 20,
                            ResultsCap = 50
                        },
                        new MonitoringConfigDto
                        {
                            Label = "google",
                            Type = MonitorType.HttpGet,
                            Address = "http://google.com",
                            Period = 20,
                            ResultsCap = 50
                        }
                    }
                }
            };

            var content = serializer.Serialize(example);
            File.WriteAllText(configName, content);
        }
    }
}
