using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("schedule-api/[controller]")]
    public class ScheduleController : ControllerBase
    {
      

        private readonly ILogger<ScheduleController> _logger;

        public ScheduleController(ILogger<ScheduleController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string  Get()
        {
            return "Hello from scheduling api!";
        }
    }
}
