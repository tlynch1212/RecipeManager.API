using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ILoggerWrapper _logger;
        private readonly IRateRepository _rateRepository;
        private readonly IUserRepository _userRepository;


        public RateController(ILoggerWrapper logger, IRateRepository rateRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _rateRepository = rateRepository;
            _userRepository = userRepository;

        }

        [HttpPost]
        public IActionResult Create([FromBody] Rating rating)
        {
            try
            {
                rating.UserId = _userRepository.GetByAuthId(rating.UserId).Id.ToString();
                _rateRepository.CreateRating(rating, true);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult Get([FromQuery]int userId, int recipeId)
        {
            return Ok(_rateRepository.Get(userId.ToString(), recipeId));
        }
    }
}