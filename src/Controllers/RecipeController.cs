using Microsoft.AspNetCore.Mvc;
using RecipeManager.Core.Models;
using RecipeManager.Core.Repositories;
using System;

namespace RecipeManager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
   // [Authorize]
    public class RecipeController : ControllerBase
    {
        private readonly ILoggerWrapper _logger;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUserRepository _userRepository;

        public RecipeController(ILoggerWrapper logger, IRecipeRepository recipeRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _recipeRepository = recipeRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Recipe recipe)
        {
            try
            {
                _recipeRepository.CreateRecipe(recipe, true);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] Recipe recipe)
        {
            try
            {
                _recipeRepository.UpdateRecipe(recipe, true);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("favorite")]
        public IActionResult Favorite([FromBody] FavoriteModel favorite)
        {
            try
            {
                var user = _userRepository.GetByAuthId(favorite.UserId);
                _recipeRepository.FavoriteRecipe(favorite.RecipeId, user.Id.ToString(), true);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("unfavorite")]
        public IActionResult UnFavorite([FromBody] FavoriteModel favorite)
        {
            try
            {
                var user = _userRepository.GetByAuthId(favorite.UserId);
                _recipeRepository.UnFavoriteRecipe(favorite.RecipeId, user.Id.ToString(), true);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] Recipe recipe)
        {
            try
            {
                _recipeRepository.DeleteRecipe(recipe, true);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("user")]
        public IActionResult Get([FromQuery] string userId)
        {
            var user = _userRepository.GetByAuthId(userId);
            return Ok(_recipeRepository.GetRecipesForUser(user.Id.ToString()));
        }

        [HttpGet]
        public IActionResult GetById([FromQuery] int recipeId)
        {
            return Ok(_recipeRepository.GetRecipeById(recipeId));
        }
    }
}