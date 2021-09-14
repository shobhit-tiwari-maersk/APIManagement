using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIManagement.Controllers
{
    [ApiController]
    [Route("gateway-api/[controller]")]
    public class ClientController : ControllerBase
    {

        private readonly ILogger<ClientController> _logger;

        private readonly IStorage _storage;

        public ClientController(ILogger<ClientController> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        [HttpPost]
        public IActionResult Add([FromBody] Client client)
        {
            return Ok(_storage.AddClient(client));
        }
    }
}
