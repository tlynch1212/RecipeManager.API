using RecipeManager.Core.Models;
using System.Collections.Generic;

namespace RecipeManager.Core.Repositories
{
    public interface IRecipeRepository
    {
        public List<Recipe> GetRecipes();
        public void CreateRecipe(Recipe recipe);
        Recipe CheckDuplication(Recipe recipe);
    }
}