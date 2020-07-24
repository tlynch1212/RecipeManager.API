using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Models;
using System;
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

        public List<Recipe> GetRecipes(int fetchCount)
        {
            return _dbContext.Recipes.Where(r => r.IsPublic == true).Take(fetchCount).Include("Ingredients").ToList();
        }

        public List<Recipe> GetRecipesForUser(string userId)
        {
            return _dbContext.Recipes.Where(t => t.UserId == userId).Include("Ingredients").ToList();
        }

        public void CreateRecipe(Recipe recipe, bool save)
        {
            recipe.CreatedDate = DateTime.Now;
            _dbContext.Recipes.Add(recipe);
            if (save)
            {
                _dbContext.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public Recipe CheckDuplication(Recipe recipe)
        {
            return _dbContext.Recipes.FirstOrDefault(t => t.Name.Equals(recipe.Name) && t.Instructions.Equals(recipe.Instructions));
        }
    }
}
