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
            return _dbContext.Recipes.Where(r => r.IsPublic == true).OrderBy(t => Guid.NewGuid()).Take(fetchCount).Include("Ingredients").Include("Instructions").ToList();
        }

        public Recipe GetRecipeById(int id)
        {
            return _dbContext.Recipes.Include("Ingredients").Include("Instructions").FirstOrDefault(t => t.Id == id);
        }

        public List<int> GetRecipeIds()
        {
            return _dbContext.Recipes.Where(r => r.IsPublic == true).OrderBy(t => Guid.NewGuid()).Select(t => t.Id).ToList();
        }

        public List<Recipe> GetRecipesForUser(string userId)
        {
            return _dbContext.Recipes.Where(t => t.UserId == userId && t.IsShared && t.SharedWith.Contains(userId)).Include("Ingredients").Include("Instructions").ToList();
        }

        public void FavoriteRecipe(int recipeId, string userId, bool save)
        {
            var recipe = _dbContext.Recipes.Find(recipeId);
            if (recipe.IsPublic)
            {
                if (recipe.SharedWith == null)
                {
                    recipe.SharedWith = new List<string>();
                }
                if (!recipe.SharedWith.Contains(userId))
                {
                    recipe.SharedWith.Add(userId);
                    _dbContext.Recipes.Update(recipe);
                    if (save)
                    {
                        _dbContext.SaveChanges();
                    }
                }
            }
        }

        public void UnFavoriteRecipe(int recipeId, string userId, bool save)
        {
            var recipe = _dbContext.Recipes.Find(recipeId);
            if (recipe.IsPublic)
            {
                recipe.SharedWith.Remove(userId);
                _dbContext.Recipes.Update(recipe);
                if (save)
                {
                    _dbContext.SaveChanges();
                }
            }
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

        public void UpdateRecipe(Recipe recipe, bool save)
        {
            _dbContext.Recipes.Update(recipe);
            if (save)
            {
                _dbContext.SaveChanges();
            }
        }

        public void DeleteRecipe(Recipe recipe, bool save)
        {
            _dbContext.Recipes.Remove(recipe);
            if (save)
            {
                _dbContext.SaveChanges();
            }
        }
    }
}
