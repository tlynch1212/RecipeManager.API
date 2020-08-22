using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManager.Core.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly RecipeManagerContext _dbContext;

        public RecipeRepository(RecipeManagerContext context)
        {
            _dbContext = context;
        }

        public List<Recipe> GetRecipes(int fetchCount)
        {
            return _dbContext.Recipes
                .Where(r => r.IsPublic)
                .OrderBy(t => Guid.NewGuid()).Take(fetchCount)
                .Include(i => i.Ingredients)
                .Include(i => i.Instructions)
                .Include(i => i.SharedWith).ToList();
        }

        public Recipe GetRecipeById(int id)
        {
            return _dbContext.Recipes
                .Include(i => i.Ingredients)
                .Include(i => i.Instructions)
                .Include(i => i.SharedWith)
                .FirstOrDefault(t => t.Id == id);
        }

        public List<int> GetRecipeIds()
        {
            return _dbContext.Recipes.Where(r => r.IsPublic).OrderBy(t => Guid.NewGuid()).Select(t => t.Id).ToList();
        }

        public List<Recipe> GetRecipesForUser(string userId)
        {
            var favoriteRecipeUsers = _dbContext.RecipeUsers.Where(t => t.User.Id.ToString() == userId).Select(s => s.Recipe.Id).ToList();

            return _dbContext.Recipes.Where(t => t.UserId == userId || favoriteRecipeUsers.Contains(t.Id))
                .Include(i => i.Ingredients).Include(i => i.Instructions).ToList();
        }

        public void FavoriteRecipe(int recipeId, string userId, bool save)
        {
            var recipe = _dbContext.Recipes.Include(i => i.SharedWith).FirstOrDefault(t => t.Id == recipeId);
            if (recipe != null && recipe.IsPublic)
            {
                if (recipe.SharedWith == null)
                {
                    recipe.SharedWith = new List<RecipeUser>();
                }

                var isAlreadyAdded = false;
                foreach (var recipeUser in recipe.SharedWith)
                {
                    if (recipeUser.User.Id.ToString() == userId)
                    {
                        isAlreadyAdded = true;
                    }
                    break;
                }

                if (!isAlreadyAdded)
                {
                    var user = _dbContext.Users.FirstOrDefault(u => u.Id.ToString() == userId);
                    recipe.SharedWith.Add(new RecipeUser
                    {
                        User = user,
                        Recipe = recipe
                    });
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
            var recipe = _dbContext.Recipes.Include(i => i.SharedWith).FirstOrDefault(t => t.Id == recipeId);
            if (recipe != null && recipe.IsPublic)
            {
                var recipeUser = _dbContext.RecipeUsers.FirstOrDefault(u => u.User.Id.ToString() == userId && u.Recipe.Id == recipeId);
                recipe.SharedWith.Remove(recipeUser);
                _dbContext.RecipeUsers.Remove(recipeUser);
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
