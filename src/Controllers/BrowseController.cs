using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeManager.Core.Interfaces;
using RecipeManager.Core.Models;

namespace RecipeManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrowseController : ControllerBase
    {
        private readonly ILogger<BrowseController> _logger;
        private readonly IRecipeRepository _recipeRepository;

        public BrowseController(ILogger<BrowseController> logger, IRecipeRepository recipeRepository)
        {
            _logger = logger;
            _recipeRepository = recipeRepository;
        }

        [HttpGet]
        public IEnumerable<Recipe> Get()
        {
            return _recipeRepository.GetRecipes();
        }
    }
}
