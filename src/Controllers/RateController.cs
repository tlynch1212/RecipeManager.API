using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeManager.Core.Models;
using RecipeManager.Core.Repositories;
using System;

namespace RecipeManager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RateController : ControllerBase
    {
        private readonly ILogger<RateController> _logger;
        private readonly IRateRepository _rateRepository;

        public RateController(ILogger<RateController> logger, IRateRepository rateRepository)
        {
            _logger = logger;
            _rateRepository = rateRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Rating rating)
        {
            try
            {
                _rateRepository.CreateRating(rating, true);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }
    }
}