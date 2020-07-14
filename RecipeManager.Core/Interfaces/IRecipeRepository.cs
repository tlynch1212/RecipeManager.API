using RecipeManager.Core.Models;
using System.Collections.Generic;

namespace RecipeManager.Core.Interfaces
{
    public interface IRecipeRepository
    {
        public List<Recipe> GetRecipes();
    }
}