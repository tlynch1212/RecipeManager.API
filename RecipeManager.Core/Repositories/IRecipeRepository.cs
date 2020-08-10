using RecipeManager.Core.Models;
using RecipeManager.Core.Recommendations.Models;
using System.Collections.Generic;

namespace RecipeManager.Core.Repositories
{
    public interface IRecipeRepository
    {
        public List<Recipe> GetRecipes(int fetchCount);
        Recipe CheckDuplication(Recipe recipe);
        void SaveChanges();
        void CreateRecipe(Recipe recipe, bool save);
        List<Recipe> GetRecipesForUser(string userId);
        void DeleteRecipe(Recipe recipe, bool save);
        void UpdateRecipe(Recipe recipe, bool save);
        List<int> GetRecipeIds();
        Recipe GetRecipeById(int id);
        void FavoriteRecipe(int recipeId, string userId, bool save);
        void UnFavoriteRecipe(int recipeId, string userId, bool save);
    }
}