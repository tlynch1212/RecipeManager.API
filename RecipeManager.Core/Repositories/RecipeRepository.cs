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
            var recipes = _dbContext.Recipes.Where(t => t.SharedWith != null).Include("Ingredients").Include("Instructions");
            var recipesShared = new List<Recipe>();
            foreach (var recipe in recipes)
            {
                if (recipe.SharedWith != null)
                {
                    if (recipe.SharedWith.Any(user => user.Id.ToString() == userId))
                    {
                        recipesShared.Add(recipe);
                    }
                }
            }
            recipesShared.AddRange(_dbContext.Recipes.Where(t => t.UserId == userId && !recipesShared.Select(r => r.Id).Contains(t.Id)).Include("Ingredients").Include("Instructions").ToList());

            return recipesShared.OrderBy(t => t.Name).ToList();
        }

        public void FavoriteRecipe(int recipeId, string userId, bool save)
        {
            var recipe = _dbContext.Recipes.Find(recipeId);
            if (recipe.IsPublic)
            {
                if (recipe.SharedWith == null)
                {
                    recipe.SharedWith = new List<User>();
                }

                var isAlreadyAdded = false;
                foreach (var user in recipe.SharedWith)
                {
                    if (user.Id.ToString() == userId)
                    {
                        isAlreadyAdded = true;
                    }
                    break;
                }

                if (!isAlreadyAdded)
                {
                    var user = _dbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId);
                    recipe.SharedWith.Add(user);
                    recipe.IsShared = true;
                    _dbContext.Recipes.Update(recipe);
                    if (save)
                    {
                        SaveChanges();
                    }
                }
            }
        }

        public void UnFavoriteRecipe(int recipeId, string userId, bool save)
        {
            var recipe = _dbContext.Recipes.Find(recipeId);
            if (recipe.IsPublic)
            {
                var user = _dbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId);
                recipe.SharedWith.Remove(user);

                if (recipe.SharedWith.Count == 0)
                {
                    recipe.IsShared = false;
                }

                _dbContext.Recipes.Update(recipe);
                if (save)
                {
                    SaveChanges();
                }
            }
        }

        public void CreateRecipe(Recipe recipe, bool save)
        {
            recipe.CreatedDate = DateTime.Now;
            _dbContext.Recipes.Add(recipe);
            if (save)
            {
                SaveChanges();
            }
        }

        public void UpdateRecipe(Recipe recipe, bool save)
        {
            _dbContext.Recipes.Update(recipe);
            if (save)
            {
                SaveChanges();
            }
        }

        public void DeleteRecipe(Recipe recipe, bool save)
        {
            _dbContext.Recipes.Remove(recipe);
            if (save)
            {
                SaveChanges();
            }
        }

        private void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
