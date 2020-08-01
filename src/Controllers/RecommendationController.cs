using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeManager.Core.Recommendations;

namespace RecipeManager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class RecommendationController : Controller
    {
        private readonly ILogger<RecommendationController> _logger;
        private readonly ITrainer _trainer;

        public RecommendationController(ILogger<RecommendationController> logger, ITrainer trainer)
        {
            _logger = logger;
            _trainer = trainer;
        }

        [HttpGet]
        public IActionResult Train()
        {
            try
            {
                return Ok(_trainer.Train());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }
    }
}
