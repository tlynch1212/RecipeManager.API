using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManager.Core.Repositories;
using System;

namespace RecipeManager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FavoriteController : ControllerBase
    {
        private readonly ILoggerWrapper _logger;
        private readonly IRecipeUserRepository _recipeUserRepository;
        private readonly IUserRepository _userRepository;


        public FavoriteController(ILoggerWrapper logger,IUserRepository userRepository, IRecipeUserRepository recipeUserRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _recipeUserRepository = recipeUserRepository;
        }

        [HttpGet]
        public IActionResult Get(string userId, int recipeId)
        {
            try
            {
                var user = _userRepository.GetByAuthId(userId);
                return Ok(_recipeUserRepository.Get(user.Id, recipeId));
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }
    }
}