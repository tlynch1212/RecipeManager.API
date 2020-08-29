using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManager.Core.Analytics;
using System;

namespace RecipeManager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly ILoggerWrapper _logger;
        private readonly IDataAggregation _dataAggregation;

        public AnalyticsController(ILoggerWrapper logger, IDataAggregation dataAggregation)
        {
            _logger = logger;
            _dataAggregation = dataAggregation;
        }

        [HttpGet("top/ratings")]
        public IActionResult GetTopRatings(int count)
        {
            try
            {
                return Ok(_dataAggregation.GetTopRated(count));
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("ratings")]
        public IActionResult GetRateDifference()
        {
            try
            {
                return Ok(_dataAggregation.GetRateDifferences());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("top/interactions")]
        public IActionResult GetTopInteractions(int count)
        {
            try
            {
                return Ok(_dataAggregation.GetMostInteraction(count));
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }
    }
}