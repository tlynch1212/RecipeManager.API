﻿using RecipeManager.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManager.Core.Repositories
{
    public class RateRepository: IRateRepository
    {
        private readonly RecipeManagerContext _dbContext;

        public RateRepository(RecipeManagerContext context)
        {
            _dbContext = context;
        }

        public List<Rating> GetAll()
        {
            return _dbContext.Ratings.ToList();
        }

        public Rating Get(string userId, int recipeId)
        {
            return _dbContext.Ratings.FirstOrDefault(r => r.UserId == userId && r.RecipeId == recipeId);
        }

        public void CreateRating(Rating rating, bool save)
        {
            _dbContext.Ratings.Add(rating);
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
