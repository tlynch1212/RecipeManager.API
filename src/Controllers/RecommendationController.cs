using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeManager.Core.Recommendations;
using RecipeManager.Core.Recommendations.Models;

namespace RecipeManager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class RecommendationController : Controller
    {
        private readonly ILogger<RecommendationController> _logger;
        private readonly ITrainer _trainer;
        private readonly IPredictor _predictor;

        public RecommendationController(ILogger<RecommendationController> logger, ITrainer trainer, IPredictor predictor)
        {
            _logger = logger;
            _trainer = trainer;
            _predictor = predictor;
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

        [HttpGet("predict")]
        public IActionResult Predict(RecipeModel recipeModel)
        {
            try
            {
                var prediction = _predictor.Predict(recipeModel);
                return Ok(prediction.Score.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }
    }
}
