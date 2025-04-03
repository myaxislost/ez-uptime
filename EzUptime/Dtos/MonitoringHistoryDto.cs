namespace EzUptime.Dtos
{
    public class MonitoringHistoryDto
    {
        public DateTime Created { get; set; }
        public IEnumerable<MonitoringStepDto> Results { get; set; }
        public MonitoringConfigDto Config { get; set; }
    }
}
