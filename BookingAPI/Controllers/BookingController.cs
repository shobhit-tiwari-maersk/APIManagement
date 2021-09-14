using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Controllers
{
    [ApiController]
    [Route("booking-api/[controller]")]
    public class BookingController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Booking"
        };

        private readonly ILogger<BookingController> _logger;

        public BookingController(ILogger<BookingController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello from Booking API!";
        }
    }
}
