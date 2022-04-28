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
        private readonly BackGroundQueue backgroundQueue;

        public HomeController(ILogger<HomeController> logger, BackGroundQueue queue)
        {
            this.logger = logger;
            backgroundQueue = queue;
        }

        [Route("index")]
        public async Task<IActionResult> Index()
        {
            await SlowMethod();
            return Ok("OK - " + DateTime.Now);
        }

        private async Task SlowMethod()
        {
            logger.LogInformation($"Starting at {DateTime.UtcNow.TimeOfDay}");
            backgroundQueue.QueueBackgroundWorkItem(async token =>
            {
                await Task.Delay(5000, token);
                logger.LogInformation($"Done at {DateTime.UtcNow.TimeOfDay}");
            });
        }
    }
}
