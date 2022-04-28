using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BackGroundServiceApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> logger;
        private readonly BackGroundWorkerQueue backgroundWorkerQueue;

        public HomeController(ILogger<HomeController> logger, BackGroundWorkerQueue queue)
        {
            this.logger = logger;
            backgroundWorkerQueue = queue;
        }

        [Route("index")]
        public async Task<IActionResult> Index()
        {
            await CallSlowApi();
            return Ok("OK - " + DateTime.Now);
        }

        private async Task CallSlowApi()
        {
            logger.LogInformation($"Starting at {DateTime.UtcNow.TimeOfDay}");
            backgroundWorkerQueue.QueueBackgroundWorkItem(async token =>
            {
                await Task.Delay(5000, token);
                logger.LogInformation($"Done at {DateTime.UtcNow.TimeOfDay}");
            });
        }
    }
}
