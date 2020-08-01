using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeManager.Core.Models;
using RecipeManager.Core.Recommendations.Models;
using RecipeManager.Core.Repositories;

namespace RecipeManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
        public IEnumerable<Recipe> Get([FromQuery] int fetchCount)
        {
            return _recipeRepository.GetRecipes(fetchCount);
        }
    }
}
