using RecipeManager.Core.Models;
using System.Collections.Generic;

namespace RecipeManager.Core.Repositories
{
    public interface IRecipeRepository
    {
        public List<Recipe> GetRecipes();
        Recipe CheckDuplication(Recipe recipe);
        void SaveChanges();
        void CreateRecipe(Recipe recipe, bool save);
        List<Recipe> GetRecipesForUser(string userId);
    }
}