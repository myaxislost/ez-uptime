
namespace EzUptime.Dtos
{
    public class MonitoringHistoryDto
    {
        public DateTime Created { get; set; }
        public IEnumerable<MonitoringStepDto> Results { get; set; }
        public MonitoringConfigDto Config { get; set; }
        public int NumErrors => Results.Where(x => !x.Success).Count();
        public double AvgPing
        {
            get
            {
                var successful = Results.Where(x => x.Success);
                if (!successful.Any())
                    return 0;
                else
                    return successful.Select(x => x.Ping).Average();
            }
        }
    }
}
