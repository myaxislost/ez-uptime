using EzUptime.Dtos;
using EzUptime.Services.Monitoring;
using Microsoft.AspNetCore.Mvc;

namespace EzUptime.Controllers
{
    [ApiController]
    [Route("api/uptime")]
    public class Uptime : ControllerBase
    {
        private readonly ILogger<Uptime> _logger;
        private readonly MonitoringService _monitor;

        public Uptime(ILogger<Uptime> logger, MonitoringService monitor)
        {
            _monitor = monitor;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var results = _monitor.Monitors.Values.Select(m => m.GetHistory());
            return Ok(results);
        }
        
    }
}
