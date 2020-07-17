using RecipeManager.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManager.Core.Repositories
{
    public class RecipeRepository: IRecipeRepository
    {
        private readonly RecipeManagerContext _dbContext;

        public RecipeRepository(RecipeManagerContext context)
        {
            _dbContext = context;
        }

        public List<Recipe> GetRecipes()
        {
            return _dbContext.Recipes.ToList();
        }

        public void CreateRecipe(Recipe recipe)
        {
            _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();
        }

        public Recipe CheckDuplication(Recipe recipe)
        {
            return _dbContext.Recipes.FirstOrDefault(t => t.Name.Equals(recipe.Name) && t.Instructions.Equals(recipe.Instructions));
        }
    }
}
