using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManager.Core.Models;
using RecipeManager.Core.Repositories;

namespace RecipeManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BrowseController : ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;

        public BrowseController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        [HttpGet]
        public IEnumerable<Recipe> Get([FromQuery] int fetchCount)
        {
            return _recipeRepository.GetRecipes(fetchCount);
        }
    }
}
