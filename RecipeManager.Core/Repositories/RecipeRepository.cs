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
            return _dbContext.Recipes.Where(r => r.IsPublic).OrderBy(t => Guid.NewGuid()).Take(fetchCount).Include("Ingredients").Include("Instructions").ToList();
        }

        public Recipe GetRecipeById(int id)
        {
            return _dbContext.Recipes.Include("Ingredients").Include("Instructions").FirstOrDefault(t => t.Id == id);
        }

        public List<int> GetRecipeIds()
        {
            return _dbContext.Recipes.Where(r => r.IsPublic).OrderBy(t => Guid.NewGuid()).Select(t => t.Id).ToList();
        }

        public List<Recipe> GetRecipesForUser(string userId)
        {
            var recipes = _dbContext.Recipes.Where(t => t.SharedWith != null).Include("Ingredients").Include("Instructions").ToList();
            var recipesShared = new List<Recipe>();
            foreach (var recipe in recipes)
            {
                if (recipe.SharedWith.Contains(userId)) {
                    recipesShared.Add(recipe);
                }

            }
            recipesShared.AddRange(_dbContext.Recipes.Where(t => t.UserId == userId && !recipesShared.Select(r => r.Id).Contains(t.Id)).Include("Ingredients").Include("Instructions").ToList());

            return recipesShared;
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
