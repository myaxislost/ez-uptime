namespace EzUptime.Dtos
{
    public class MonitoringStepDto
    {
        public DateTime Timestamp { get; set; }
        public bool Success { get; set; }
        public double Ping { set; get; }
    }
}
