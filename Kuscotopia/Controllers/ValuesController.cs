using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Kuscotopia.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kuscotopia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly QueueService queueService;

        public ValuesController(QueueService queueService)
        {
            this.queueService = queueService;
        }
        
        [HttpPost("{WorkerCount:int}")]
        public async Task<IActionResult> PostAsync(int WorkerCount)
        {
            if(WorkerCount < 0 || WorkerCount > 1000)
            {
                return StatusCode((int)HttpStatusCode.NotAcceptable);
            }
            await this.queueService.QueueWorkAsync(WorkerCount);

            return StatusCode((int)HttpStatusCode.Accepted);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return null;
        }
    }
}
