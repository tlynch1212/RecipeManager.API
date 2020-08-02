﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeManager.Core.Recommendations;
using RecipeManager.Core.Repositories;

namespace RecipeManager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class RecommendationController : Controller
    {
        private readonly ILogger<RecommendationController> _logger;
        private readonly ITrainer _trainer;
        private readonly IPredictor _predictor;
        private readonly IUserRepository _userRepository;

        public RecommendationController(ILogger<RecommendationController> logger, ITrainer trainer, IPredictor predictor, IUserRepository userRepository)
        {
            _logger = logger;
            _trainer = trainer;
            _predictor = predictor;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Train([FromQuery] int iterations)
        {
            try
            {
                return Ok(_trainer.Train(iterations));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("predict")]
        public IActionResult Predict([FromQuery] string authId, int amount)
        {
            try
            {
                var user = _userRepository.GetByAuthId(authId);
                var recommendations = _predictor.Predict(user.Id, amount);
                return Ok(recommendations);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }
    }
}
