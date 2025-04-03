namespace EzUptime.Dtos
{
    public class MonitoringHistoryDto
    {
        public DateTime Created { get; set; }
        public IEnumerable<MonitoringStepDto> Resutls { get; set; }
        public MonitoringConfigDto ConfigDto { get; set; }
    }
}
