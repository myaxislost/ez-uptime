namespace EzUptime.Dtos
{
    public enum MonitorType
    {
        None = 0,
        HttpGet = 1,
        Ping = 2,
    }

    public class MonitoringConfigDto
    {
        public string Label { get; set; }
        public MonitorType Type { get; set; }
        public string Address { get; set; }
        public double Period { get; set; } = 30;
        public int? ResultsCap { get; set; } = 40;
    }
}
